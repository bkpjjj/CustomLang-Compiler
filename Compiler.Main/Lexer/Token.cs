namespace Compiler.Main.Lex
{
    public enum TokenType
    {
        TT_INT,
        TT_FLOAT,
        TT_PLUS,
        TT_MINUS,
        TT_MUL,
        TT_DIV,
        TT_LPAREN,
        TT_RPAREN,
        TT_LCBRACE,
        TT_RCBRACE,
        TT_LSBRACKET,
        TT_RSBRACLET,
        TT_KEYWORD,
        TT_IDENTIFIER,
        TT_COMMA,
        TT_EQ,
        TT_EOL,
        TT_STRING,
        TT_NOT
    }

    public static class KeyWords
    {
        public readonly static string[] Items =
        {
            "class",
            "def",
            "let",
            "include",
            "pass"
        };
    }
    public class Token
    {
        public TokenType TokenType { get; set; }

        public string Value { get; set; }

        public TokenPosition Position { get; set; }

        public Token(TokenType tokenType,TokenPosition position, string value = null)
        {
            TokenType = tokenType;
            Value = value;
            Position = position;
        }

        public bool Match(TokenType type , string value = null)
        {
            if (value == null)
                return TokenType == type;
            return TokenType == type && Value == value;
        }

        public bool Match(Token token)
        {
            return Match(token.TokenType, token.Value);
        }
        public override string ToString()
        {
            if(Value != null)
                return $"{TokenType}:{Value}";
            else
                return $"{TokenType}";
        }
    }
}