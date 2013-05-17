using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KingSurvivalGame;
using System.IO;
using System.Collections.Generic;

namespace TestKingSurvivalGame
{
    [TestClass]
    public class TestEngine
    {
        // TODO: more full game scenarios

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestEngineConstructorSetNullValue_ThrowsException()
        {
            Engine currentEngine = new Engine(null, null);
        }
        [TestMethod]

        public void TestPositiveKingExit()
        {
            List<Figure> figures = new List<Figure>();
            figures.Add(new Pawn(new Position(0, 0), 'A'));
            figures.Add(new Pawn(new Position(0, 2), 'B'));
            figures.Add(new Pawn(new Position(0, 4), 'C'));
            figures.Add(new Pawn(new Position(0, 6), 'D'));
            figures.Add(new King(new Position(7, 3)));
            GameBoard gameBoard = new GameBoard(figures);

            Engine currentEngine = new Engine(gameBoard, figures);
            int currentKingRow = 2;
            currentEngine.CheckForKingExit(currentKingRow);
            Assert.IsFalse(currentEngine.GameIsInProgress, "King wins for sure!Game over");

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestInputCommandIsNull_ThrowsException()
        {
            string num = null;
            Console.SetIn(new StringReader(num));
            List<Figure> figures = new List<Figure>();
            figures.Add(new Pawn(new Position(0, 0), 'A'));
            figures.Add(new Pawn(new Position(0, 2), 'B'));
            figures.Add(new Pawn(new Position(0, 4), 'C'));
            figures.Add(new Pawn(new Position(0, 6), 'D'));
            figures.Add(new King(new Position(3, 7)));
            GameBoard gameBoard = new GameBoard(figures);

            Engine currentEngine = new Engine(gameBoard, figures);
            currentEngine.ProcessASide("King");
        }

        [TestMethod]
        public void TestInputCommandNameKingIsEligible()
        {
           
            List<Figure> figures = new List<Figure>();
            figures.Add(new Pawn(new Position(0, 0), 'A'));
            figures.Add(new Pawn(new Position(0, 2), 'B'));
            figures.Add(new Pawn(new Position(0, 4), 'C'));
            figures.Add(new Pawn(new Position(0, 6), 'D'));
            figures.Add(new King(new Position(3, 7)));
            GameBoard gameBoard = new GameBoard(figures);
            Engine currentEngine = new Engine(gameBoard, figures);
            string command = "KUL";
            Assert.IsTrue(currentEngine.ValidateCommand(command), "Command is valid named");
        }

        [TestMethod]
        public void TestInputCommandNameKingIsNotEligible()
        {
           
            List<Figure> figures = new List<Figure>();
            figures.Add(new Pawn(new Position(0, 0), 'A'));
            figures.Add(new Pawn(new Position(0, 2), 'B'));
            figures.Add(new Pawn(new Position(0, 4), 'C'));
            figures.Add(new Pawn(new Position(0, 6), 'D'));
            figures.Add(new King(new Position(3, 7)));
            GameBoard gameBoard = new GameBoard(figures);

            Engine currentEngine = new Engine(gameBoard, figures);
            string command = "KURT";
            currentEngine.MoveCounter = 1;

            Assert.IsFalse(currentEngine.ValidateCommand(command), "Command is not valid named");
        }

        [TestMethod]
        public void TestInputCommandForPawnIsEligible()
        {
                List<Figure> figures = new List<Figure>();
                figures.Add(new Pawn(new Position(0, 0), 'A'));
                figures.Add(new Pawn(new Position(0, 2), 'B'));
                figures.Add(new Pawn(new Position(0, 4), 'C'));
                figures.Add(new Pawn(new Position(0, 6), 'D'));
                figures.Add(new King(new Position(3, 7)));
                GameBoard gameBoard = new GameBoard(figures);
                Engine currentEngine = new Engine(gameBoard, figures);
                currentEngine.MoveCounter = 3;
                string currentComand = "ADR";
                Assert.IsTrue(currentEngine.ValidateCommand(currentComand), "Command is valid named");
        }
        [TestMethod]
        public void TestInputCommandForPawnIsNotEligible()
        {
            List<Figure> figures = new List<Figure>();
            figures.Add(new Pawn(new Position(0, 0), 'A'));
            figures.Add(new Pawn(new Position(0, 2), 'B'));
            figures.Add(new Pawn(new Position(0, 4), 'C'));
            figures.Add(new Pawn(new Position(0, 6), 'D'));
            figures.Add(new King(new Position(3, 7)));
            GameBoard gameBoard = new GameBoard(figures);
            Engine currentEngine = new Engine(gameBoard, figures);
            currentEngine.MoveCounter = 3;
            string currentComand = "ADRt";
            Assert.IsFalse(currentEngine.ValidateCommand(currentComand), "Command is not valid named");
        }

        [TestMethod]
        public void TestKingImpossibleMove()
        {
            List<Figure> figures = new List<Figure>();
            figures.Add(new Pawn(new Position(0, 0), 'A'));
            figures.Add(new Pawn(new Position(0, 2), 'B'));
            figures.Add(new Pawn(new Position(0, 4), 'C'));
            figures.Add(new Pawn(new Position(0, 6), 'D'));
            King newKing=new King(new Position(3, 7));
            figures.Add(newKing);
            GameBoard gameBoard = new GameBoard(figures);
            Engine currentEngine = new Engine(gameBoard, figures);
            Assert.IsNull(currentEngine.GetNewCoordinates(newKing,Direction.UR), "You can't move here");
        }

        [TestMethod]
        public void TestKingPossibleMove()
        {
            List<Figure> figures = new List<Figure>();
            figures.Add(new Pawn(new Position(0, 0), 'A'));
            figures.Add(new Pawn(new Position(0, 2), 'B'));
            figures.Add(new Pawn(new Position(0, 4), 'C'));
            figures.Add(new Pawn(new Position(0, 6), 'D'));
            King newKing = new King(new Position(3, 7));
            figures.Add(newKing);
            GameBoard gameBoard = new GameBoard(figures);
            Engine currentEngine = new Engine(gameBoard, figures);
            bool isNewPosition=(currentEngine.GetNewCoordinates(newKing, Direction.UL))is Position ;

            Assert.IsTrue(isNewPosition, "You can move here");
        }
        [TestMethod]
        public void TestPawnImpossibleOutOfBorderMove()
        {
            List<Figure> figures = new List<Figure>();
            figures.Add(new Pawn(new Position(0, 0), 'A'));
            figures.Add(new Pawn(new Position(0, 2), 'B'));
            figures.Add(new Pawn(new Position(0, 4), 'C'));
            figures.Add(new Pawn(new Position(0, 6), 'D'));
            King newKing = new King(new Position(3, 7));
            figures.Add(newKing);
            GameBoard gameBoard = new GameBoard(figures);
            Engine currentEngine = new Engine(gameBoard, figures);
            Assert.IsNull(currentEngine.GetNewCoordinates(newKing, Direction.UR), "You can't move here");
        }

        [TestMethod]
        public void TestPawnImpossibleMove()
        {
            List<Figure> figures = new List<Figure>();
            figures.Add(new Pawn(new Position(0, 0), 'A'));
            figures.Add(new Pawn(new Position(0, 2), 'B'));
            figures.Add(new Pawn(new Position(0, 4), 'C'));
            Pawn newPawn = new Pawn(new Position(3, 4), 'D');
            figures.Add(newPawn);
            King newKing = new King(new Position(4, 5));
            figures.Add(newKing);
            GameBoard gameBoard = new GameBoard(figures);
            Engine currentEngine = new Engine(gameBoard, figures);
            Position newPosition = currentEngine.GetNewCoordinates(newPawn, Direction.DR) ;

            Assert.IsNull(newPosition, "You can't move here,cell is busy");
        }

        [TestMethod]
        public void TestPawnUpMoveInTableBorders()
        {
            List<Figure> figures = new List<Figure>();
            figures.Add(new Pawn(new Position(0, 0), 'A'));
            figures.Add(new Pawn(new Position(0, 2), 'B'));
            figures.Add(new Pawn(new Position(0, 4), 'C'));
            Pawn newPawn = new Pawn(new Position(3, 4), 'D');
            figures.Add(newPawn);
            King newKing = new King(new Position(4, 5));
            figures.Add(newKing);
            GameBoard gameBoard = new GameBoard(figures);
            Engine currentEngine = new Engine(gameBoard, figures);
            Position newPosition = currentEngine.GetNewCoordinates(newPawn, Direction.UR) ;

            Assert.IsFalse(currentEngine.ValidateCommand("DUR"), "You can't move in this direction");
        }


        [TestMethod]
        public void TestScenario_KingWinsIn7Moves()
        {
            //string moves = string.Format("kul{0}bdr{0}kul{0}bdl{0}kur{0}adr{0}kur{0}bdl{0}kur{0}bdl{0}kul{0}ddl{0}kul{0}", Environment.NewLine);
            string moves = System.IO.File.ReadAllText("inputScenario_KingWinsIn7Moves.txt");
            using (StringWriter stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);

                using (StringReader stringReader = new StringReader(moves))
                {
                    Console.SetIn(stringReader);
                    List<Figure> figures = new List<Figure>();
                    figures.Add(new Pawn(new Position(0, 0), 'A')); //2,4
                    figures.Add(new Pawn(new Position(0, 2), 'B')); //2,8
                    figures.Add(new Pawn(new Position(0, 4), 'C')); //2,12
                    figures.Add(new Pawn(new Position(0, 6), 'D')); //2,16
                    //figures.Add(new Pawn(new Position(2, 6), 'Z'));
                    figures.Add(new King(new Position(7, 3))); //9, 10
                    GameBoard gameBoard = new GameBoard(figures);

                    Engine engine = new Engine(gameBoard, figures); // adding everything to the engine

                    engine.Run(); // executing the game

                    string[] outputLines = stringWriter.ToString().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    string expected = "King wins in 7 moves!";
                    string actual = outputLines[outputLines.Length - 1];

                    Assert.AreEqual(expected, actual);
                }
            }
        }
    }
}
