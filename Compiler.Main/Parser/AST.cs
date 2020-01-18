using Compiler.Main.Errors;
using Compiler.Main.Lex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Main.Parser
{

    class ASTTree
    {
        public ASTNode Root { get; set; }

        public ASTTree(ASTNode root)
        {
            Root = root;
        }

        public override string ToString()
        {
            if (Root != null)
                return $"[{Root}]";
            return "[]";
        }
    }

    public abstract class ASTNode
    {
        
    }

    public class ASTAtom : ASTNode
    {
        public Token Token { get; set; }

        public ASTAtom(Token token)
        {
            Token = token;
        }

        public override string ToString()
        {
            return $"{Token}";
        }
    }

    public class ASTBinOp : ASTNode
    {
        public ASTNode Left { get; set; }

        public Token Token { get; set; }

        public ASTNode Right { get; set; }

        public ASTBinOp(ASTNode left, Token Op, ASTNode right)
        {
            Left = left;
            Token = Op;
            Right = right;
        }

        public override string ToString()
        {
            return $"({Left},{Token},{Right})";
        }
    }

    class AST
    {
        private Token[] tokens;
        private NodePosition position;
        private Token current_token { get { return tokens[position.Index]; } set { current_token = value; } }

        public AST(Token[] tokens)
        {
            this.tokens = tokens;
        }

        public bool Next()
        {
            if (position.Index < tokens.Length - 1)
            {
                position.Next();
                return true;
            }
            return false;
        }

        public ASTTree GetASTTree(out Error error)
        {
            error = null;
            ASTTree tree = new ASTTree(expr());
            if (tree.Root == null)
                error = new IlligalSyntaxError($"{current_token}", current_token.Position);
            return tree;
        }

        private ASTNode term() => BinOp(atom, new[] { TokenType.TT_MUL, TokenType.TT_DIV });

        private ASTNode expr() => BinOp(term, new[] { TokenType.TT_PLUS, TokenType.TT_MINUS });

        private ASTNode BinOp(Func<ASTNode> func,TokenType[] ops)
        {
            var left = func();
            ASTNode result = left;
            TokenType[] binOpTypes = ops;
            while (binOpTypes.Any(x => current_token.Match(x)))
            {
                var token = current_token;
                Next();
                var right = func();
                result = new ASTBinOp(left, token, right);
            }
            return result;
        }

        private ASTNode atom()
        {
            Token tok = current_token;
            TokenType[] valueTypes = { TokenType.TT_INT, TokenType.TT_FLOAT, TokenType.TT_STRING };
            if (valueTypes.Any(x => current_token.Match(x)))
            {
                Next();
                return new ASTAtom(tok);
            }
            return null;
        }
    }
}
