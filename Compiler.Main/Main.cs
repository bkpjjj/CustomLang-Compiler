using Compiler.Main.Errors;
using Compiler.Main.Lex;
using Compiler.Main.Parser;
using System;
using System.IO;


namespace Compiler.Main
{
    class Program
    {

        public static void WriteTokens(Token[] tokens)
        {
            Console.WriteLine("[");

            foreach (Token t in tokens)
            {
                if (t.TokenType == TokenType.TT_KEYWORD)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(t);
                }
                else if (t.TokenType == TokenType.TT_IDENTIFIER)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(t);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(t);
                }
            }

            Console.WriteLine("]");
        }
        static void Main(string[] args)
        {
#if DEBUG
            string code = File.ReadAllText(@"E:\source\repos\CML\Compiler.Main\test.cml");
#else
            string code = args[0];
#endif

            Lexer lex = new Lexer(code);

            Token[] tokens = lex.GetTokens(out Error error);

            if(error != null)
            {
                Console.WriteLine(error);
            }
            else
            {
                WriteTokens(tokens);

                AST parser = new AST(tokens);

                ASTTree tree = parser.GetASTTree(out Error treeError);
                if (treeError != null)
                    Console.WriteLine(treeError);
                else
                    Console.WriteLine(tree);
            }

            Console.ReadKey();
        }
    }
}
