/*
 * Lexer.cs
 *
 * This file is used internally during parsing of the code, breaking source
 * code text into a series of tokens.
 * 
 * Unless you’re writing a fancy MiniScript code editor, you probably don’t 
 * need to worry about this stuff.
 */

using System;
using System.Collections.Generic;
using MiniScriptSharp.Constants;
using MiniScriptSharp.Errors;
using MiniScriptSharp.Lexis;
using MiniScriptSharp.Tests;
using static MiniScriptSharp.Constants.Consts;

namespace MiniScriptSharp.Lexis {
				
	public class Lexer {
		public int LineNum = 1; // start at 1, so we report 1-based line numbers

		public int Position;
		private string input;
		private int inputLength;

		private Queue<Token> pending;

		public bool AtEnd => Position >= inputLength && pending.Count == 0;

		public Lexer(string input) {
			this.input = input;
			inputLength = input.Length;
			Position = 0;
			pending = new Queue<Token>();
		}

		public Token Peek() {
			if (pending.Count != 0) return pending.Peek();
			
			if (AtEnd) return Token.Eol;
			
			pending.Enqueue(Dequeue());
			return pending.Peek();
		}

		public Token Dequeue() {
			if (pending.Count > 0) return pending.Dequeue();

			var oldPos = Position;
			SkipWhitespaceAndComment();

			if (AtEnd) return Token.Eol;

			var result = new Token {AfterSpace = (Position > oldPos)};
			var startPos = Position;
			var c = input[Position++];

			// Handle two-character operators first.
			if (!AtEnd) {
				var c2 = input[Position];
				result.Type = c switch {
					'=' when c2 == '=' => TokenType.OpEqual,
					'!' when c2 == '=' => TokenType.OpNotEqual,
					'>' when c2 == '=' => TokenType.OpGreatEqual,
					'<' when c2 == '=' => TokenType.OpLessEqual,
					_ => result.Type
				};

				if (result.Type != TokenType.Unknown) {
					Position++;
					return result;
				}
			}

			// Handle one-char operators next.
			result.Type = c switch {
				'+' => TokenType.OpPlus,
				'-' => TokenType.OpMinus,
				'*' => TokenType.OpTimes,
				'/' => TokenType.OpDivide,
				'%' => TokenType.OpMod,
				'^' => TokenType.OpPower,
				'(' => TokenType.LParen,
				')' => TokenType.RParen,
				'[' => TokenType.LSquare,
				']' => TokenType.RSquare,
				'{' => TokenType.LCurly,
				'}' => TokenType.RCurly,
				',' => TokenType.Comma,
				':' => TokenType.Colon,
				'=' => TokenType.OpAssign,
				'<' => TokenType.OpLesser,
				'>' => TokenType.OpGreater,
				'@' => TokenType.AddressOf,
				_ => result.Type
			};
			
			switch (c) {
				case ';':
				case '\n': {
					result.Type = TokenType.EOL;
					result.Text = c == ';' ? ";" : "\n";
					if (c != ';') LineNum++;
					break;
				}
				case '\r': {
					// Careful; DOS may use \r\n, so we need to check for that too.
					result.Type = TokenType.EOL;
					if (Position < inputLength && input[Position] == '\n') {
						Position++;
						result.Text = "\r\n";
					} else {
						result.Text = "\r";
					}
					LineNum++;
					break;
				}
			}

			if (result.Type != TokenType.Unknown) return result;

			// Then, handle more extended tokens.

			if (c == '.') {
				// A token that starts with a dot is just Type.Dot, UNLESS
				// it is followed by a number, in which case it's a decimal number.
				if (Position >= inputLength || !IsNumeric(input[Position])) {
					result.Type = TokenType.Dot;
					return result;
				}
			}

			if (c == '.' || IsNumeric(c)) {
				result.Type = TokenType.Number;
				while (Position < inputLength) {
					var lastCh = c;
					c = input[Position];
					if (IsNumeric(c) || c == '.' || c == 'E' || c == 'e' ||
					    ((c == '-' || c == '+') && (lastCh == 'E' || lastCh == 'e'))) {
						Position++;
					} else break;
				}
				
			} else if (IsIdentifier(c)) {
				while (Position < inputLength) {
					if (IsIdentifier(input[Position])) Position++;
					else break;
				}
				result.Text = input.Substring(startPos, Position - startPos);
				result.Type = (Keywords.IsKeyword(result.Text) ? TokenType.Keyword : TokenType.Identifier);
				switch (result.Text) {
					case END: {
						// As a special case: when we see "end", grab the next keyword (after whitespace)
						// too, and conjoin it, so our token is "end if", "end function", etc.
						var nextWord = Dequeue();
						if (nextWord != null && nextWord.Type == TokenType.Keyword) {
							result.Text = result.Text + " " + nextWord.Text;
						} else {
							// Oops, didn't find another keyword.  User error.
							throw new LexerException("'end' without following keyword ('if', 'function', etc.)");
						}
						break;
					}
					case ELSE: {
						// And similarly, conjoin an "if" after "else" (to make "else if").
						var p = Position;
						while (p < inputLength && (input[p]==' ' || input[p]=='\t')) p++;
						if (p+1 < inputLength && input.Substring(p,2) == Consts.IF &&
						    (p+2 >= inputLength || IsWhitespace(input[p+2]))) {
							result.Text = ELSE_IF;
							Position = p + 2;
						}
						break;
					}
				}
				return result;
				
			} else if (c == '"') {
				// Lex a string... to the closing ", but skipping (and singling) a doubled double quote ("")
				result.Type = TokenType.String;
				var haveDoubledQuotes = false;
				startPos = Position;
				var gotEndQuote = false;
				while (Position < inputLength) {
					c = input[Position++];
					if (c != '"') continue;
					
					if (Position < inputLength && input[Position] == '"') {
						// This is just a doubled quote.
						haveDoubledQuotes = true;
						Position++;
					} else {
						// This is the closing quote, marking the end of the string.
						gotEndQuote = true;
						break;
					}
				}
				if (!gotEndQuote) throw new LexerException("missing closing quote (\")");
				
				result.Text = input.Substring(startPos, Position-startPos-1);
				if (haveDoubledQuotes) result.Text = result.Text.Replace("\"\"", "\"");
				return result;

			} else {
				result.Type = TokenType.Unknown;
			}

			result.Text = input.Substring(startPos, Position - startPos);
			return result;
		}

		private void SkipWhitespaceAndComment() {
			while (!AtEnd && IsWhitespace(input[Position])) {
				Position++;
			}

			if (Position < input.Length - 1 && input[Position] == '/' && input[Position + 1] == '/') {
				// Comment.  Skip to end of line.
				Position += 2;
				while (!AtEnd && input[Position] != '\n') Position++;
			}
		}
		
		public static bool IsNumeric(char c) {
			return c >= '0' && c <= '9';
		}

		public static bool IsIdentifier(char c) {
			return c == '_'
				|| (c >= 'a' && c <= 'z')
				|| (c >= 'A' && c <= 'Z')
				|| (c >= '0' && c <= '9')
				|| c > '\u009F';
		}

		public static bool IsWhitespace(char c) {
			return c == ' ' || c == '\t';
		}
		
		public bool IsAtWhitespace() {
			// Caution: ignores queue, and uses only current position
			return AtEnd || IsWhitespace(input[Position]);
		}

		public static bool IsInStringLiteral(int charPos, string source, int startPos=0) {
			var inString = false;
			for (int i=startPos; i<charPos; i++) {
				if (source[i] == '"') inString = !inString;
			}
			return inString;
		}

		public static int CommentStartPos(string source, int startPos) {
			// Find the first occurrence of "//" in this line that
			// is not within a string literal.
			var commentStart = startPos-2;
			while (true) {
				commentStart = source.IndexOf("//", commentStart + 2, StringComparison.Ordinal);
				if (commentStart < 0) break;	// no comment found
				if (!IsInStringLiteral(commentStart, source, startPos)) break;	// valid comment
			}
			return commentStart;
		}
		
		public static string TrimComment(string source) {
			var startPos = source.LastIndexOf('\n') + 1;
			var commentStart = CommentStartPos(source, startPos);
			return commentStart >= 0 ? source.Substring(startPos, commentStart - startPos) : source;
		}

		// Find the last token in the given source, ignoring any whitespace
		// or comment at the end of that line.
		public static Token LastToken(string source) {
			// Start by finding the start and logical  end of the last line.
			var startPos = source.LastIndexOf('\n') + 1;
			var commentStart = CommentStartPos(source, startPos);
			
			// Walk back from end of string or start of comment, skipping whitespace.
			var endPos = (commentStart >= 0 ? commentStart-1 : source.Length - 1);
			while (endPos >= 0 && IsWhitespace(source[endPos])) endPos--;
			if (endPos < 0) return Token.Eol;
			
			// Find the start of that last token.
			// There are several cases to consider here.
			var tokStart = endPos;
			var c = source[endPos];
			if (IsIdentifier(c)) {
				while (tokStart > startPos && IsIdentifier(source[tokStart-1])) tokStart--;
			} else if (c == '"') {
				var inQuote = true;
				while (tokStart > startPos) {
					tokStart--;
					if (source[tokStart] == '"') {
						inQuote = !inQuote;
						if (!inQuote && tokStart > startPos && source[tokStart-1] != '"') break;
					}
				}
			} else if (c == '=' && tokStart > startPos) {
				var c2 = source[tokStart-1];
				if (c2 == '>' || c2 == '<' || c2 == '=' || c2 == '!') tokStart--;
			}
			
			// Now use the standard lexer to grab just that bit.
			var lex = new Lexer(source) {Position = tokStart};
			return lex.Dequeue();
		}

		public static void Check(Token tok, TokenType type, string text=null, int lineNum=0) {
			UnitTest.ErrorIfNull(tok);
			if (tok == null) return;
			
			UnitTest.ErrorIf(tok.Type != type, $"Token type: expected {type}, but got {tok.Type}");
			UnitTest.ErrorIf(text != null && tok.Text != text, $"Token text: expected {text}, but got {tok.Text}");
		}

		public static void CheckLineNum(int actual, int expected) {
			UnitTest.ErrorIf(actual != expected, $"Lexer line number: expected {expected}, but got {actual}");
		}

		public static void RunUnitTests() {
			var lex = new Lexer("42  * 3.14158");
			Check(lex.Dequeue(), TokenType.Number, "42");
			CheckLineNum(lex.LineNum, 1);
			Check(lex.Dequeue(), TokenType.OpTimes);
			Check(lex.Dequeue(), TokenType.Number, "3.14158");
			UnitTest.ErrorIf(!lex.AtEnd, "AtEnd not set when it should be");
			CheckLineNum(lex.LineNum, 1);

			lex = new Lexer("6*(.1-foo) end if // and a comment!");
			Check(lex.Dequeue(), TokenType.Number, "6");
			CheckLineNum(lex.LineNum, 1);
			Check(lex.Dequeue(), TokenType.OpTimes);
			Check(lex.Dequeue(), TokenType.LParen);
			Check(lex.Dequeue(), TokenType.Number, ".1");
			Check(lex.Dequeue(), TokenType.OpMinus);
			Check(lex.Peek(), TokenType.Identifier, "foo");
			Check(lex.Peek(), TokenType.Identifier, "foo");
			Check(lex.Dequeue(), TokenType.Identifier, "foo");
			Check(lex.Dequeue(), TokenType.RParen);
			Check(lex.Dequeue(), TokenType.Keyword, "end if");
			Check(lex.Dequeue(), TokenType.EOL);
			UnitTest.ErrorIf(!lex.AtEnd, "AtEnd not set when it should be");
			CheckLineNum(lex.LineNum, 1);

			lex = new Lexer("\"foo\" \"isn't \"\"real\"\"\" \"now \"\"\"\" double!\"");
			Check(lex.Dequeue(), TokenType.String, "foo");
			Check(lex.Dequeue(), TokenType.String, "isn't \"real\"");
			Check(lex.Dequeue(), TokenType.String, "now \"\" double!");
			UnitTest.ErrorIf(!lex.AtEnd, "AtEnd not set when it should be");

			lex = new Lexer("foo\nbar\rbaz\r\nbamf");
			Check(lex.Dequeue(), TokenType.Identifier, "foo");
			CheckLineNum(lex.LineNum, 1);
			Check(lex.Dequeue(), TokenType.EOL);
			Check(lex.Dequeue(), TokenType.Identifier, "bar");
			CheckLineNum(lex.LineNum, 2);
			Check(lex.Dequeue(), TokenType.EOL);
			Check(lex.Dequeue(), TokenType.Identifier, "baz");
			CheckLineNum(lex.LineNum, 3);
			Check(lex.Dequeue(), TokenType.EOL);
			Check(lex.Dequeue(), TokenType.Identifier, "bamf");
			CheckLineNum(lex.LineNum, 4);
			Check(lex.Dequeue(), TokenType.EOL);
			UnitTest.ErrorIf(!lex.AtEnd, "AtEnd not set when it should be");
			
			Check(LastToken("x=42 // foo"), TokenType.Number, "42");
			Check(LastToken("x = [1, 2, // foo"), TokenType.Comma);
			Check(LastToken("x = [1, 2 // foo"), TokenType.Number, "2");
			Check(LastToken("x = [1, 2 // foo // and \"more\" foo"), TokenType.Number, "2");
			Check(LastToken("x = [\"foo\", \"//bar\"]"), TokenType.RSquare);
			Check(LastToken("print 1 // line 1\nprint 2"), TokenType.Number, "2");			
			Check(LastToken("print \"Hi\"\"Quote\" // foo bar"), TokenType.String, "Hi\"Quote");			
		}
	}
}

