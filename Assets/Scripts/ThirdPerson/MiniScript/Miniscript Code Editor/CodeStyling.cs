/* This file defines markup tags applied to style MiniScript code,
and the C# code needed to actually apply that markup.  If you're
using a classic Text object for your source code lines, then stick
to the basic <b>, <i>, and <color> tags.  If you're using TMPro,
then you have access to pretty much any of these:
	http://digitalnativestudios.com/textmeshpro/docs/rich-text/
*/
using MiniScriptSharp.Errors;
using MiniScriptSharp.Lexis;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MiniscriptSharp.CodeEdit {
	
public class CodeStyling : MonoBehaviour {
	
	[System.Serializable]
	public class Style {
		public string startTags;
		public string endTags;
		
		public Style(string startTags, string endTags) {
			this.startTags = startTags;
			this.endTags = endTags;
		}
	}
	
	[Header("Styles")]
	public Style identifier = new Style("", "");
	public Style operators = new Style("<color=#4444AA>", "</color>");
	public Style stringLiteral = new Style("<color=#AA4444><noparse>", "</noparse></color>");
	public Style comment = new Style("<i><color=#666666><noparse>", "</noparse></i></color>");
	public Style numericLiteral = new Style("<color=#44AA44>", "</color>");
	public Style keyword = new Style("<color=#AA44AA>", "</color>");
	public Style openString = new Style("<color=#CC0000><noparse>", "</noparse></color>");
	public Style colon = new Style("<color=#FF00FF>", "</color>");
	
	[Header("Other Options")]
	public bool rotatingParenColors = true;
	public Color baseParenColor = new Color(0, 0, 0.8f);
	public bool rotatingSquareColors = true;
	public Color baseSquareColor = new Color(0.1f, 0.1f, 0.5f);
	
	public string Markup(string code) {
		var sb = new StringBuilder();
		var lexer = new Lexer(code);
		int parenDepth = 0, squareDepth = 0;
		
		while (!lexer.AtEnd) {
			int start = lexer.Position;
			
			// grab whitespace (normally skipped by the lexer)
			if (Lexer.IsWhitespace(code[lexer.Position])) {
				while (!lexer.AtEnd && Lexer.IsWhitespace(code[lexer.Position])) lexer.Position++;
				sb.Append(code.Substring(start, lexer.Position - start));
				if (lexer.AtEnd) break;
				start = lexer.Position;
			}
			
			// also check for a comment (which the lexer would also skip over)
			if (lexer.Position < code.Length-2 && code[lexer.Position]=='/' && code[lexer.Position+1]=='/') {
				while (!lexer.AtEnd && code[lexer.Position] != '\n') lexer.Position++;
				sb.Append(comment.startTags);
				sb.Append(code.Substring(start, lexer.Position - start));
				sb.Append(comment.endTags);
				if (lexer.AtEnd) break;
				start = lexer.Position;
			}
			
			// now, grab and process the next token (being sure to catch and handle lexer exceptions)
			Token tok = null;
			try {
				tok = lexer.Dequeue();
			} catch (LexerException exc) {
				tok = new Token();
				lexer.Position = code.Length;
			}
			
			if (tok.Text == "self") tok.Type = TokenType.Keyword;	// (special case)
			
			switch (tok.Type) {
			case TokenType.Keyword:
				sb.Append(keyword.startTags);
				sb.Append(tok.Text);
				sb.Append(keyword.endTags);
				break;
			case TokenType.Colon:
				sb.Append(colon.startTags);
				sb.Append(":");
				sb.Append(colon.endTags);
				break;
			case TokenType.Identifier:
				sb.Append(identifier.startTags);
				sb.Append(tok.Text);
				sb.Append(identifier.endTags);
				break;
			case TokenType.String:
				sb.Append(stringLiteral.startTags);
				sb.Append("\"");	// (note that lexer strips the surrounding quotes)
				sb.Append(tok.Text.Replace("\"", "\"\""));	// and un-doubles internal quotes
				sb.Append("\"");
				sb.Append(stringLiteral.endTags);
				break;
			case TokenType.Number:
				sb.Append(numericLiteral.startTags);
				sb.Append(tok.Text);
				sb.Append(numericLiteral.endTags);
				break;
			case TokenType.LParen:
			case TokenType.RParen:
				if (tok.Type == TokenType.LParen) parenDepth++;
				if (rotatingParenColors) {
					float h, s, v;
					Color.RGBToHSV(baseParenColor, out h, out s, out v);
					h = Mathf.Repeat(h + 0.22f * (parenDepth-1), 1);
					Color color = Color.HSVToRGB(h, s, v);
					if (parenDepth < 1) color = Color.red;
					sb.Append("<color=#");
					sb.Append(ColorUtility.ToHtmlStringRGB(color));
					sb.Append(tok.Type == TokenType.LParen ? ">(</color>" : ">)</color>");
				} else {
					sb.Append(tok.Type == TokenType.LParen ? ">(</color>" : ">)</color>");					
				}
				if (tok.Type == TokenType.RParen) parenDepth--;
				break;
			case TokenType.LSquare:
			case TokenType.RSquare:
				if (tok.Type == TokenType.LSquare) squareDepth++;
				if (rotatingSquareColors) {
					float h, s, v;
					Color.RGBToHSV(baseSquareColor, out h, out s, out v);
					h = Mathf.Repeat(h + 0.22f * (squareDepth-1), 1);
					Color color = Color.HSVToRGB(h, s, v);
					if (squareDepth < 1) color = Color.red;
					sb.Append("<color=#");
					sb.Append(ColorUtility.ToHtmlStringRGB(color));
					sb.Append(tok.Type == TokenType.LSquare ? ">[</color>" : ">]</color>");
				} else {
					sb.Append(tok.Type == TokenType.LSquare ? ">[</color>" : ">]</color>");					
				}
				if (tok.Type == TokenType.RSquare) squareDepth--;
				break;
			case TokenType.Unknown:
				if (code[start] == '"') {
					sb.Append(openString.startTags);
					sb.Append(code.Substring(start, lexer.Position - start));
					sb.Append(openString.endTags);
				} else {
					sb.Append(code.Substring(start, lexer.Position - start));					
				}
				break;
			default:
				sb.Append(operators.startTags);
				sb.Append(code.Substring(start, lexer.Position - start));
				sb.Append(operators.endTags);
				break;
			}
		}
		return sb.ToString();
	}
}
	
}