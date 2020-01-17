using Compiler.Main.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Main.Lex
{
    class Lexer
    {
        private string code;
        private Position position;
        private char current_char { get { return code[position.Index]; } set { current_char = value; } }

        public Lexer(string code)
        {
            this.code = code;
        }

        public bool Next()
        {
            if (position.Index < code.Length - 1)
            {
                position.Next(current_char);
                return true;
            }
            return false;
        }

        public Token[] GetTokens(out Error error)
        {
            error = null;
            List<Token> tokens = new List<Token>();
            do
            {
                if (char.IsLetter(current_char))
                    tokens.Add(MakeWordToken());
                if (char.IsDigit(current_char))
                    tokens.Add(MakeDigitToken());
                switch (current_char)
                {
                    case ' ':
                    case '\n':
                    case '\r':
                    case '\t':
                        continue;
                    case '+':
                        tokens.Add(new Token(TokenType.TT_PLUS));
                        break;
                    case '-':
                        tokens.Add(new Token(TokenType.TT_MINUS));
                        break;
                    case '*':
                        tokens.Add(new Token(TokenType.TT_MUL));
                        break;
                    case '/':
                        tokens.Add(new Token(TokenType.TT_DIV));
                        break;
                    case '{':
                        tokens.Add(new Token(TokenType.TT_LCBRACE));
                        break;
                    case '}':
                        tokens.Add(new Token(TokenType.TT_RSBRACLET));
                        break;
                    case '[':
                        tokens.Add(new Token(TokenType.TT_LSBRACKET));
                        break;
                    case ']':
                        tokens.Add(new Token(TokenType.TT_RSBRACLET));
                        break;
                    case '(':
                        tokens.Add(new Token(TokenType.TT_LPAREN));
                        break;
                    case ')':
                        tokens.Add(new Token(TokenType.TT_RPAREN));
                        break;
                    case ',':
                        tokens.Add(new Token(TokenType.TT_COMMA));
                        break;
                    case '=':
                        tokens.Add(new Token(TokenType.TT_EQ));
                        break;
                    case ';':
                        tokens.Add(new Token(TokenType.TT_EOL));
                        break;
                    default:
                        error = new IlligalCharError($" '{current_char}'", position);
                        return null;
                }
            } while (Next());
            return tokens.ToArray();
        }

        private Token MakeDigitToken()
        {
            string word = "";
            int dotCount = 0;
            bool canNext = true;
            while ((char.IsDigit(current_char) || current_char == '.') && canNext)
            {
                if (current_char == '.')
                    dotCount++;
                if (dotCount > 1)
                    break;
                word += current_char;

                canNext = Next();
            }
            return dotCount == 0 ? new Token(TokenType.TT_INT, word) : new Token(TokenType.TT_FLOAT, word);
        }

        private Token MakeWordToken()
        {
            string word = "";
            bool canNext = true;
            while (char.IsLetterOrDigit(current_char) && canNext)
            {
                word += current_char;

                canNext = Next();
            }
            if (KeyWords.Items.Any(x => x == word))
                return new Token(TokenType.TT_KEYWORD, word);
            else
                return new Token(TokenType.TT_IDENTIFIER, word);

        }
    }
}
