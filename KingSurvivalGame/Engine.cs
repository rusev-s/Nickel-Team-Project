using System;
using System.Collections.Generic;
using System.Linq;

namespace KingSurvivalGame
{
    public class Engine
    {
        private int moveCounter = 0;
        private bool gameIsInProgress = true;
        private bool kingHasAvailableMoves = false;
        private bool pawnsHaveAvailableMoves = false;
        private readonly GameBoard gameBoard;
        private readonly List<Figure> figures;
        private readonly List<char> charRepresentationsPawns;
        private readonly char kingSymbol = 'K';
        internal int MoveCounter { get; set; }
        internal bool GameIsInProgress { get; set; }
        internal bool KingHasAvailableMoves { get; set; }
        internal bool PawnsHaveAvailableMoves { get; set; }
        public Engine(GameBoard gameBoard, List<Figure> figures)
        {
            this.gameBoard = gameBoard;
            this.figures = figures;
            this.charRepresentationsPawns = new List<char>();

            foreach (var figure in this.figures)
            {
                if (figure is Pawn)
                {
                    charRepresentationsPawns.Add(figure.SymbolRepresentation);
                }
            }
        }

        public void Run()
        {
            while (gameIsInProgress)
            {
                if (moveCounter % 2 == 0)
                {
                    gameBoard.DrawGameBoard();
                    this.ProcessKingSide();
                }
                else
                {
                    gameBoard.DrawGameBoard();
                    this.ProcessPawnSide();
                }

            }
        }

        public void ProcessKingSide()
        {
            ProcessASide("King");
        }

        public void ProcessPawnSide()
        {
            ProcessASide("Pawn");
        }

        internal void ProcessASide(string side)
        {
            bool isValidCommand = false;
            while (!isValidCommand)
            {
                if (side == "King")
                {
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.Write("Please enter king's turn: ");
                }
                else if (side == "Pawn")
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Write("Please enter pawn's turn: ");
                }

                Console.ResetColor();
                string input = Console.ReadLine();

                if (input != null)
                {
                    input = input.ToUpper();
                    isValidCommand = ValidateCommand(input);
                    if (isValidCommand)
                    {
                        ProcessCommand(input);
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid command name!");
                        Console.ResetColor();
                    }
                }
                else
                {
                    isValidCommand = false;
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Please enter a valid command!");
                    Console.ResetColor();
                }
            }
        }

        private bool ValidateCommand(string commandToCheck)
        {
            bool commandIsValid = false;

            if (this.moveCounter % 2 == 0) //king's move
            {
                if (commandToCheck == kingSymbol + Direction.DL.ToString() ||
                    commandToCheck == kingSymbol + Direction.DR.ToString() ||
                    commandToCheck == kingSymbol + Direction.UL.ToString() ||
                    commandToCheck == kingSymbol + Direction.UR.ToString())
                {
                    commandIsValid = true;
                }
            }
            else if (this.moveCounter % 2 != 0) //pawn's move
            {
                foreach (var letterRepresentation in charRepresentationsPawns)
                {
                    if (commandToCheck == letterRepresentation + Direction.DL.ToString() ||
                        commandToCheck == letterRepresentation + Direction.DR.ToString())
                    {
                        commandIsValid = true;
                        break;
                    }
                }
            }

            return commandIsValid;
        }

        private void ProcessCommand(string input)
        {
            char figureLetter = input[0];
            Figure currentFigure = null;
            foreach (var figure in this.figures)
            {
                if (figure.SymbolRepresentation == figureLetter)
                {
                    currentFigure = figure;
                }
            }

            string commandDirection = input.Substring(1, 2);
            Direction direction = GetDirection(commandDirection);
            Position currentPosition = currentFigure.Position;

            while (currentPosition != null)
            {
                currentPosition = GetNewCoordinates(currentFigure, direction); //returns null for invalid coordinates
                if (currentPosition != null) //we found valid coordinates
                {
                    UpdateGameField(currentFigure, direction); // this moves the char
                    currentFigure.Position = currentPosition; // this changes the position
                    UpdateAllAvailableMoves(); // we moved a figure and update available moves for all figures
                    SetFiguresHaveAvailableMoves();
                    CheckForFiguresBlocked();
                    break;
                }
            }
        }

        private Direction GetDirection(string commandDirection)
        {
            Direction direction = default(Direction);
            switch (commandDirection)
            {
                case "DL":
                    direction = Direction.DL;
                    break;
                case "DR":
                    direction = Direction.DR;
                    break;
                case "UL":
                    direction = Direction.UL;
                    break;
                case "UR":
                    direction = Direction.UR;
                    break;
                default:
                    break;
            }

            return direction;
        }

        /// <summary>
        /// Gets the displacement for figure movement
        /// </summary>
        private Position GetDisplacement(Direction direction)
        {
            Position displacement = null;
            switch (direction)
            {
                case Direction.DL:
                    displacement = new Position(1, -2);
                    break;
                case Direction.DR:
                    displacement = new Position(1, 2);
                    break;
                case Direction.UL:
                    displacement = new Position(-1, -2);
                    break;
                case Direction.UR:
                    displacement = new Position(-1, 2);
                    break;
                default:
                    break;
            }

            return displacement;
        }

        private bool IsValidGameBoardCell(Position position)
        {
            bool valid = this.gameBoard.IsInGameField(position) && this.gameBoard[position.Row, position.Column] == ' ';
            return valid;
        }

        private Position GetNewCoordinates(Figure currentFigure, Direction direction)
        {
            Position currentPosition = currentFigure.Position;
            Position displacement = GetDisplacement(direction);
            Position newPosition = currentPosition + displacement;

            if (IsValidGameBoardCell(newPosition))
            {
                return newPosition;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("You can't move there!");
                Console.ResetColor();
                return null;
            }
        }

        private void UpdateGameField(Figure currentFigure, Direction direction)
        {
            Position oldPosition = currentFigure.Position;
            Position displacement = GetDisplacement(direction);
            Position newPosition = oldPosition + displacement;
            char sign = gameBoard[oldPosition.Row, oldPosition.Column];
            gameBoard[oldPosition.Row, oldPosition.Column] = ' ';
            gameBoard[newPosition.Row, newPosition.Column] = sign;
            this.moveCounter++; //we processed a valid command so the moves increment

            CheckForKingExit(newPosition.Row);
        }

        private void UpdateAllAvailableMoves()
        {
            foreach (Figure figure in this.figures)
            {
                if (figure.GetType() == typeof(Pawn))
                {
                    Position downLeft = figure.Position + GetDisplacement(Direction.DL);
                    Position downRight = figure.Position + GetDisplacement(Direction.DR);
                    Position[] neighbours = { downLeft, downRight };
                    for (int i = 0; i < neighbours.Length; i++)
                    {
                        if (!IsValidGameBoardCell(neighbours[i]))
                        {
                            (figure as Pawn).ExistingMoves[i] = false;
                        }
                        else
                        {
                            (figure as Pawn).ExistingMoves[i] = true;
                        }
                    }
                }
                else if (figure.GetType() == typeof(King))
                {
                    Position downLeft = figure.Position + GetDisplacement(Direction.DL);
                    Position downRight = figure.Position + GetDisplacement(Direction.DR);
                    Position upLeft = figure.Position + GetDisplacement(Direction.UL);
                    Position upRight = figure.Position + GetDisplacement(Direction.UR);
                    Position[] neighbours = { downLeft, downRight, upLeft, upRight };
                    for (int i = 0; i < neighbours.Length; i++)
                    {
                        if (!IsValidGameBoardCell(neighbours[i]))
                        {
                            (figure as King).ExistingMoves[i] = false;
                        }
                        else
                        {
                            (figure as King).ExistingMoves[i] = true;
                        }
                    }
                }
            }
        }

        private void SetFiguresHaveAvailableMoves()
        {
            var allPawns = from pawn in this.figures
                           where (pawn.GetType() == typeof(Pawn))
                           select pawn;
            pawnsHaveAvailableMoves = false;

            foreach (var pawn in allPawns)
            {
                foreach (var move in (pawn as Pawn).ExistingMoves)
                {
                    if (move == true)
                    {
                        pawnsHaveAvailableMoves = true;
                    }
                }
            }

            var kings = from king in this.figures
                        where (king.GetType() == typeof(King))
                        select king;

            kingHasAvailableMoves = false;
            foreach (var king in kings)
            {
                foreach (var move in (king as King).ExistingMoves)
                {
                    if (move == true)
                    {
                        kingHasAvailableMoves = true;
                    }
                }
            }
        }

        internal void CheckForFiguresBlocked()
        {
            if (!pawnsHaveAvailableMoves)
            {
                Console.WriteLine("End!");
                Console.WriteLine("All pawns are blocked! King wins in {0} moves!", (moveCounter / 2) + 1); //added one for the last move
                this.gameIsInProgress = false;
            }

            if (!kingHasAvailableMoves)
            {
                Console.WriteLine("King is blocked! King loses in {0} moves!", (moveCounter / 2) + 1);
                this.gameIsInProgress = false;
            }
        }

        internal void CheckForKingExit(int currentKingRow)
        {
            if (currentKingRow == 2) //actually gameBoard.HeightPadding
            {
                Console.WriteLine("End!");
                Console.WriteLine("King wins in {0} moves!", (moveCounter / 2) + 1); //added one for the last move
                this.gameIsInProgress = false;
            }

        }
    }
}

