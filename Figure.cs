using System;

namespace KingSurvivalGame
{
    public abstract class Figure
    {
        public Position Position { get; set; }
        public char SymbolRepresentation { get; set; }

        public Figure(Position position, char symbol)
        {
            this.Position = position;
            this.SymbolRepresentation = symbol;
        }
    }
}
