using System;
using System.Collections.Generic;
using System.Linq;

namespace KingSurvivalGame
{
    public class GameBoard
    {
        public GameBoard(List<Figure> figures)
        {
            CheckFiguresInput(figures);
            InitializeGameField(figures);
        }

        public char this[int i, int j]
        {
            get { return this.gameBoard[i, j]; }
            set { this.gameBoard[i, j] = value; }
        }

        private const int Height = 8;
        private const int Width = 8;
        private const int PaddingHeight = 2;
        private const int PaddingWidth = 4;
        private const int PaddingBetweenFigures = TotalWidth / Width;   //try with different widths
        private const int TotalHeight = Height + 2 * PaddingHeight;
        private const int TotalWidth = 2 * Width + 2 * PaddingWidth - 1;

        private static readonly Position topLeft = new Position(PaddingHeight, PaddingWidth);
        private static readonly Position topRight = new Position(PaddingHeight, (PaddingWidth + 2 * Width) - 2); // 2 is for the right padding
        private static readonly Position bottomLeft = new Position(Height + PaddingHeight - 1, PaddingWidth); //-1 because it is 0 based
        private static readonly Position bottomRight = new Position(Height + PaddingHeight - 1, (PaddingWidth + 2 * Width) - 2);

        private char[,] gameBoard;

        //public int BoardHeight
        //{
        //    get { return Height; }
        //}

        //public int BoardWidth
        //{
        //    get { return Width; }
        //}

        private void InitializeGameField(List<Figure> figures)
        {
            this.gameBoard = new char[TotalHeight, TotalWidth];

            //all cells are empty now
            for (int row = 0; row < TotalHeight; row++)
            {
                for (int col = 0; col < TotalWidth; col++)
                {
                    this.gameBoard[row, col] = ' ';
                }
            }

            //put figures            
            foreach (var figure in figures)
            {
                figure.Position.Row += PaddingHeight;
                figure.Position.Column *= PaddingBetweenFigures;
                figure.Position.Column += PaddingWidth;

                if (IsInGameField(figure.Position))
                {
                    this.gameBoard[figure.Position.Row, figure.Position.Column] = figure.SymbolRepresentation;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Invalid figure position!");
                }
            }

            //left and right borders
            for (int row = PaddingHeight; row <= TotalHeight - PaddingHeight; row++)
            {
                this.gameBoard[row, PaddingWidth - 2] = '|';
                this.gameBoard[row, TotalWidth - PaddingWidth + 1] = '|';
            }

            //top and bottom borders
            for (int col = PaddingWidth; col < TotalWidth - PaddingWidth; col++)
            {
                this.gameBoard[PaddingHeight - 1,col] = '_';
                this.gameBoard[Height + PaddingHeight, col] = '_';
            }

            //left and right numerations
            char rowNumber = 'a';
            for (int row = PaddingHeight; row < PaddingHeight + Height; row++)
            {
                this.gameBoard[row, 0] = rowNumber;
                this.gameBoard[row, TotalWidth - 1] = rowNumber;
                rowNumber++;
            }

            //top and bottom numerations
            char colNumber = 'A';
            for (int col = PaddingWidth; col < TotalWidth - PaddingWidth; col += PaddingBetweenFigures)
            {
                this.gameBoard[0, col] = colNumber;
                this.gameBoard[TotalHeight - 1, col] = colNumber;
                colNumber++;
            }

            //directions chars
            this.gameBoard[0, 0] = 'U';
            this.gameBoard[0, 1] = 'L';
            this.gameBoard[0, TotalWidth - 2] = 'U';
            this.gameBoard[0, TotalWidth - 1] = 'R';
            this.gameBoard[TotalHeight - 1, 0] = 'D';
            this.gameBoard[TotalHeight - 1, 1] = 'L';
            this.gameBoard[TotalHeight - 1, TotalWidth - 2] = 'D';
            this.gameBoard[TotalHeight - 1, TotalWidth - 1] = 'R';
        }

        public bool IsInGameField(Position position)
        {
            int positonRow = position.Row;
            bool isRowInBoard = (positonRow >= topLeft.Row) && (positonRow <= bottomLeft.Row);
            int positonCol = position.Column;
            bool isColInBoard = (positonCol >= topLeft.Column) && (positonCol <= topRight.Column);
            bool isValidPosition = isRowInBoard && isColInBoard;

            return isValidPosition;
        }

        public void DrawGameBoard()
        {
            Console.WriteLine();
            for (int row = 0; row < gameBoard.GetLength(0); row++)
            {
                for (int col = 0; col < gameBoard.GetLength(1); col++)
                {
                    Position positionCell = new Position(row, col);
                    bool isCellIn = IsInGameField(positionCell);
                    if (isCellIn)
                    {
                        if (row % 2 == 0)
                        {
                            if (col % 4 == 0)
                            {
                                Console.BackgroundColor = ConsoleColor.Green;
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.Write(gameBoard[row, col]);
                                Console.ResetColor();
                            }
                            else if (col % 2 == 0)
                            {
                                Console.BackgroundColor = ConsoleColor.Blue;
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.Write(gameBoard[row, col]);
                                Console.ResetColor();
                            }
                            else //if (col % 2 != 0)
                            {
                                Console.Write(gameBoard[row, col]);
                            }
                        }
                        else if (col % 4 == 0)
                        {
                            Console.BackgroundColor = ConsoleColor.Blue;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write(gameBoard[row, col]);
                            Console.ResetColor();
                        }
                        else if (col % 2 == 0)
                        {
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write(gameBoard[row, col]);
                            Console.ResetColor();
                        }
                        else //if (col % 2 != 0)
                        {
                            Console.Write(gameBoard[row, col]);
                        }
                    }
                    else
                    {
                        Console.Write(gameBoard[row, col]);
                    }

                }
                Console.WriteLine();
                Console.ResetColor();
            }
            Console.WriteLine();
        }

        private void CheckFiguresInput(List<Figure> figures)
        {
            if (figures.Count == 0)
            {
                throw new MissingMemberException("No figures on the board!");
            }
            CheckIfThereIsOneKing(figures);
            CheckIfThereArePawns(figures);
            CheckIfThereAreFiguresWithTheSameName(figures);
        }

        private void CheckIfThereAreFiguresWithTheSameName(List<Figure> figures)
        {
            List<char> pawnsNames = new List<char>();
            foreach (var figure in figures)
            {
                if (figure.GetType()==typeof(Pawn))
                {
                    if (figure.SymbolRepresentation=='K') // if a pawn is represented with 'K' as king
                    {
                        throw new ArgumentException("There cannot be pawn represented with 'K' as the king!");
                    }
                    pawnsNames.Add(figure.SymbolRepresentation);
                }
            }

            if (pawnsNames.Count != pawnsNames.Distinct().Count()) // there are duplicates
            {
                throw new ArgumentException("There cannot be two or more pawns with the same name!");
            }
        }

        private void CheckIfThereArePawns(List<Figure> figures)
        {
            int numberOfPawns = 0;
            foreach (var figure in figures)
            {
                if (figure.GetType() == typeof(Pawn))
                {
                    numberOfPawns++;
                }
            }
            if (numberOfPawns == 0)
            {
                throw new ArgumentOutOfRangeException("No pawns on the board");
            }
        }

        private void CheckIfThereIsOneKing(List<Figure> figures)
        {
            int numberOfKings = 0;
            foreach (var figure in figures)
            {
                if (figure.GetType() == typeof(King))
                {
                    numberOfKings++;
                }
            }
            if (numberOfKings != 1)
            {
                throw new ArgumentOutOfRangeException("Wrong number of kings!");
            }
        }
    }
}
