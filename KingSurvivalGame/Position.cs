using System;

namespace KingSurvivalGame
{
    public class Position
    {
        public int Row { set; get; }
        public int Column { set; get; }

        public Position(int row, int col)
        {
            this.Row = row;
            this.Column = col;
        }

        public static Position operator + (Position first, Position second)
        {
            return new Position(first.Row + second.Row , first.Column + second.Column);
        }
    }
}
