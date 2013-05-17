using System;

namespace KingSurvivalGame
{
    public class Pawn : Figure
    {
        public Pawn(Position position, char pawnSymbol)
            : base(position, pawnSymbol)
        {
            this.ExistingMoves = new bool[] { true, true };
        }

        public bool[] ExistingMoves { get; set; }
    }
}
