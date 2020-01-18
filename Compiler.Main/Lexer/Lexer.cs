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
        private TokenPosition position;
        private char current_char { get { return code[position.Index]; } set { current_char = value; } }
        private bool canNext;
        public Lexer(string code)
        {
            this.code = code;
            if (code.Length > 0)
                canNext = true;
            else
                canNext = false;
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
                if (current_char == '"')
                    tokens.Add(MakeStringToken());
                if (char.IsLetter(current_char) || current_char == '_')
                    tokens.Add(MakeWordToken());
                if (char.IsDigit(current_char))
                    tokens.Add(MakeDigitToken());
                if (!canNext)
                    break;
                switch (current_char)
                {
                    case ' ':
                    case '\n':
                    case '\r':
                    case '\t':
                        continue;
                    case '+':
                        tokens.Add(new Token(TokenType.TT_PLUS,position));
                        break;
                    case '-':
                        tokens.Add(new Token(TokenType.TT_MINUS, position));
                        break;
                    case '*':
                        tokens.Add(new Token(TokenType.TT_MUL, position));
                        break;
                    case '/':
                        tokens.Add(new Token(TokenType.TT_DIV, position));
                        break;
                    case '{':
                        tokens.Add(new Token(TokenType.TT_LCBRACE, position));
                        break;
                    case '}':
                        tokens.Add(new Token(TokenType.TT_RSBRACLET, position));
                        break;
                    case '[':
                        tokens.Add(new Token(TokenType.TT_LSBRACKET, position));
                        break;
                    case ']':
                        tokens.Add(new Token(TokenType.TT_RSBRACLET, position));
                        break;
                    case '(':
                        tokens.Add(new Token(TokenType.TT_LPAREN, position));
                        break;
                    case ')':
                        tokens.Add(new Token(TokenType.TT_RPAREN, position));
                        break;
                    case ',':
                        tokens.Add(new Token(TokenType.TT_COMMA, position));
                        break;
                    case '=':
                        tokens.Add(new Token(TokenType.TT_EQ, position));
                        break;
                    case ';':
                        tokens.Add(new Token(TokenType.TT_EOL, position));
                        break;
                    case '!':
                        tokens.Add(new Token(TokenType.TT_NOT, position));
                        break;
                    default:
                        error = new IlligalCharError($" '{current_char}'", position);
                        return null;
                }      
            } while (Next());
            return tokens.ToArray();
        }
        
        private Token MakeStringToken()
        {
            string word = "";
            Next();
            while (current_char != '"' && canNext) 
            {
                word += current_char;

                canNext = Next();
            } 
            Next();
            return new Token(TokenType.TT_STRING, position, word);
        }

        private Token MakeDigitToken()
        {
            string word = "";
            int dotCount = 0;
            do
            {
                if (current_char == '.')
                    dotCount++;
                if (dotCount > 1)
                    break;
                word += current_char;
                canNext = Next();
            } while ((char.IsDigit(current_char) || current_char == '.') && canNext);
            return dotCount == 0 ? new Token(TokenType.TT_INT, position, word) : new Token(TokenType.TT_FLOAT, position, word);
        }

        private Token MakeWordToken()
        {
            string word = "";
            while ((char.IsLetterOrDigit(current_char) || current_char == '_') && canNext)
            {
                word += current_char;

                canNext = Next();
            }
            if (KeyWords.Items.Any(x => x == word))
                return new Token(TokenType.TT_KEYWORD, position, word);
            else
                return new Token(TokenType.TT_IDENTIFIER, position, word);

        }
    }
}
