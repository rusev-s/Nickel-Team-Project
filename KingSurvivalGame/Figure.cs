using System;

namespace KingSurvivalGame
{
    public abstract class Figure
    {
        public Position Position { get; set; }
        private char symbolRepresentation;

        public Figure(Position position, char symbol)
        {
            this.Position = position;

            //if (((int)symbol >= (int)'a') && ((int)'z' <= (int)symbol))) || 
            //    ((int)symbol >= (int)'A' &&  (int)'Z' <= (int)symbol))
            if ((symbol >= 'A' && symbol <= 'Z') || (symbol >= 'a' && symbol <= 'z'))
            {
                this.symbolRepresentation = symbol;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Not valid figure symbol");
            }
        }

        public char SymbolRepresentation
        {
            get
            {
                return this.symbolRepresentation;
            }
            private set
            {
            }
        }
    }
}
