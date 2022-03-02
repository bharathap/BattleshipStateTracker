using System;

namespace Battleship.Core.Exceptions
{
    public class BattleshipValidationException : Exception
    {
        public BattleshipValidationException()
        {
        }

        public BattleshipValidationException(string message) : base(message)
        {
        }
    }
}
