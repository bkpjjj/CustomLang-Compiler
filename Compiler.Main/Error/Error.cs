using Compiler.Main.Lex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Main.Errors
{
    class Error
    {
        public string Message { get; set; }

        public Position Position { get; private set; }

        public Error(string message)
        {
            Message = message;
        }

        public Error(string message, Position position) : this(message)
        {
            Position = position;
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}:{Message} at {Position}";
        }
    }

    class IlligalCharError : Error
    {
        public IlligalCharError(string message, Position position) : base(message, position)
        {
        }
    }
}
