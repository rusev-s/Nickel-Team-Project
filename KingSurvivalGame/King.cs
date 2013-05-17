using System;

namespace KingSurvivalGame
{
    public class King : Figure
    {
        private static readonly char kingSymbol = 'K';

        public King(Position position)
            : base(position, kingSymbol)
        {
            this.ExistingMoves = new bool[] { true, true, true, true };
        }

        public bool[] ExistingMoves { get; set; }
    }
}
