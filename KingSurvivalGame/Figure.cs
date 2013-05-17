using System;

namespace KingSurvivalGame
{
    public abstract class Figure
    {
        public Position Position { get; set; }
        private char symbolRepresentation;

        public Figure(Position position,char symbolRepresentation)
        {
            this.Position = position;
            this.SymbolRepresentation = symbolRepresentation;
        }

        public char SymbolRepresentation
        {
            get
            {
                return this.symbolRepresentation; 
            }
            set
            {
                if (((int)value < 65 && 90 < (int)value) || ((int)value < 97 && 123 < (int)value))
                {
                    throw new ArgumentOutOfRangeException("Not valid figure symbol");
                }
                this.symbolRepresentation = value;
            }
        }               
    }
}
