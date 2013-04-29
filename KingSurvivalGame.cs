using System;
using System.Collections.Generic;

namespace KingSurvivalGame
{
    class KingSurvivalGame
    {
        static void Main(string[] args)
        {   
            List<Figure> figures = new List<Figure>();
            //figures coordinates are 0-based
            figures.Add(new Pawn(new Position(0, 0), 'A')); //2,4
            figures.Add(new Pawn(new Position(0, 2), 'B')); //2,8
            figures.Add(new Pawn(new Position(0, 4), 'C')); //2,12
            figures.Add(new Pawn(new Position(0, 6), 'D')); //2,16
            //figures.Add(new Pawn(new Position(2, 6), 'Z'));
            figures.Add(new King(new Position(7, 3))); //9, 10
            GameBoard gameBoard = new GameBoard(figures);
            Engine engine = new Engine(gameBoard, figures);
            engine.Run();
            
            Console.WriteLine("\nThank you for using this game!\n\n");
        }
    }
}
