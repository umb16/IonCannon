namespace MiniScriptSharp.Lexis {

    public class Token {
        
        public static readonly Token Eol = new Token(TokenType.EOL);

        public TokenType Type;
        public string Text; // may be null for things like operators, whose text is fixed
        public bool AfterSpace;

        public Token(TokenType type = TokenType.Unknown, string text = null) {
            this.Type = type;
            this.Text = text;
        }

        public override string ToString() {
            return Text == null ? Type.ToString() : $"{Type}({Text})";
        }

    }

}