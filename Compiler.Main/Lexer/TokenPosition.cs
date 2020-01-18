using System;

namespace Compiler.Main.Lex
{
    public struct TokenPosition
    {
        public int Index { get; private set; }
        public int Line { get; private set; }

        public void Next(char _char)
        {
            Index++;
            if (_char == '\n')
                Line++;
        }

        public override string ToString()
        {
            return $"{{Line:{Line + 1},Index:{Index + 1}}}";
        }
    }
}