using System;
using System.Collections.Generic;
using System.Linq;

namespace KingSurvivalGame
{
    public class Engine
    {
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
        internal bool IsValidCommand { get; set; }

        public Engine(GameBoard gameBoard, List<Figure> figures)
        {
            this.gameBoard = gameBoard;
            this.figures = figures;
            this.charRepresentationsPawns = new List<char>();
            this.GameIsInProgress = true;
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
            while (GameIsInProgress)
            {
                if (this.MoveCounter % 2 == 0)
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
            this.IsValidCommand = false;
            while (!this.IsValidCommand)
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
                    this.IsValidCommand = ValidateCommand(input);
                    if (this.IsValidCommand)
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
                    throw new ArgumentNullException("Input is null!");
                }
            }
        }

        internal bool ValidateCommand(string commandToCheck)
        {
            bool commandIsValid = false;

            if (this.MoveCounter % 2 == 0) //king's move
            {
                if (commandToCheck == kingSymbol + Direction.DL.ToString() ||
                    commandToCheck == kingSymbol + Direction.DR.ToString() ||
                    commandToCheck == kingSymbol + Direction.UL.ToString() ||
                    commandToCheck == kingSymbol + Direction.UR.ToString())
                {
                    commandIsValid = true;
                }
            }
            else if (this.MoveCounter % 2 != 0) //pawn's move
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
                return commandIsValid;
            }

            return commandIsValid;
        }

        /// <summary>
        /// The main logic of the game is here. 
        /// Processes given command. If the command is valid
        /// the char representation of figure is moved and 
        /// also the coordinates of it are updated. After that
        /// there is a check if the moved figure blocks any other figure
        /// and in the end there is a check if the game ends because of 
        /// blocked figure.
        /// </summary>
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
                //returns null for invalid coordinates
                currentPosition = GetNewCoordinates(currentFigure, direction);
                //we found valid coordinates
                if (currentPosition != null)
                {
                    // this moves the char representation of the figure
                    UpdateGameField(currentFigure, direction);
                    // this changes the position
                    currentFigure.Position = currentPosition;
                    // we moved a figure and update available moves for all figures
                    UpdateAllAvailableMoves();
                    // check if figures have available moves and for end game
                    SetFiguresHaveAvailableMoves();
                    CheckForFiguresBlocked();
                    break;
                }
            }
        }

        /// <summary>
        /// Gets the direction for the figure from given string input
        /// </summary>
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
                    throw new ArgumentException("Invalid direction!");
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
                    throw new ArgumentException("Invalid direction for displacement!");
            }

            return displacement;
        }

        private bool IsValidGameBoardCell(Position position)
        {
            bool valid = this.gameBoard.IsInGameField(position) && this.gameBoard[position.Row, position.Column] == ' ';
            return valid;
        }

        internal Position GetNewCoordinates(Figure currentFigure, Direction direction)
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
            this.MoveCounter++; //we processed a valid command so the moves increment

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
                Console.WriteLine("All pawns are blocked! King wins in {0} moves!", (MoveCounter / 2) + 1); //added one for the last move
                this.GameIsInProgress = false;
            }

            if (!kingHasAvailableMoves)
            {
                Console.WriteLine("King is blocked! King loses in {0} moves!", (MoveCounter / 2) + 1);
                this.GameIsInProgress = false;
            }
        }

        internal void CheckForKingExit(int currentKingRow)
        {
            if (currentKingRow == 2) //actually gameBoard.HeightPadding
            {
                Console.WriteLine("End!");
                //added one for the last move
                Console.WriteLine("King wins in {0} moves!", (this.MoveCounter / 2) + 1);
                this.GameIsInProgress = false;
            }

        }
    }
}


