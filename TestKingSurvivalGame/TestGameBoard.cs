using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KingSurvivalGame;
using System.Collections.Generic;

namespace TestKingSurvivalGame
{
    [TestClass]
    public class TestGameBoard
    {

        //TODO: Figure not on the right color field;        

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestGameBoard_IfKingStartsAtTheSameRowWithThePawns()
        {
            List<Figure> figures = new List<Figure>();
            // adding pawns
            figures.Add(new Pawn(new Position(3, 0), 'A'));
            figures.Add(new Pawn(new Position(4, 2), 'B'));
            figures.Add(new Pawn(new Position(5, 4), 'C'));
            figures.Add(new Pawn(new Position(5, 6), 'D'));
            // adding king
            figures.Add(new King(new Position(3, 8)));        

            GameBoard gameBoard = new GameBoard(figures);
        }

        [TestMethod]        
        public void TestGameBoard_Indexer()
        {
            List<Figure> figures = new List<Figure>();
            figures.Add(new Pawn(new Position(0, 0), 'A'));
            figures.Add(new King(new Position(1, 1))); // if there is no king added there is another exception
            GameBoard gameBoard = new GameBoard(figures);
            gameBoard[1, 1] = '3';
            Assert.AreEqual('3', gameBoard[1, 1], "GameBoard indexer is not working correctly");
            Assert.AreEqual('A', gameBoard[0, 0+4], "GameBoard indexer is not working correctly"); // +4 because there are some symbols outside the border
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestGameBoard_PawnWithSymbolRepresentationAsKing()
        {
            List<Figure> figures = new List<Figure>();
            figures.Add(new Pawn(new Position(0, 0), 'K'));            
            figures.Add(new King(new Position(1,1))); // if there is no king added there is another exception
            GameBoard gameBoard = new GameBoard(figures);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestGameBoard_FiguresWithTheSameName()
        {
            List<Figure> figures = new List<Figure>();
            figures.Add(new King(new Position(1, 1))); // if there is no king added there is another exception
            figures.Add(new Pawn(new Position(0, 0), 'A')); //2,4
            figures.Add(new Pawn(new Position(0, 2), 'B')); //2,8
            figures.Add(new Pawn(new Position(0, 4), 'C')); //2,12
            figures.Add(new Pawn(new Position(0, 6), 'B')); //2,16

            GameBoard gameBoard = new GameBoard(figures);
        }

        [TestMethod]
        [ExpectedException(typeof(MissingMemberException))]
        public void TestGameBoard_NoFiguresOnTheBoard()
        {
            GameBoard board = new GameBoard(new List<Figure>());            
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestGameBoard_NoKingOnTheBoard()
        {
            // adding only pawns
            List<Figure> figures = new List<Figure>();
            figures.Add(new Pawn(new Position(0, 0), 'A')); //2,4
            figures.Add(new Pawn(new Position(0, 2), 'B')); //2,8
            figures.Add(new Pawn(new Position(0, 4), 'C')); //2,12
            figures.Add(new Pawn(new Position(0, 6), 'D')); //2,16

            GameBoard gameBoard = new GameBoard(figures);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestGameBoard_MoreThanOneKingOnTheBoard()
        {            
            List<Figure> figures = new List<Figure>();
            // adding pawns
            figures.Add(new Pawn(new Position(0, 0), 'A')); //2,4
            figures.Add(new Pawn(new Position(0, 2), 'B')); //2,8
            figures.Add(new Pawn(new Position(0, 4), 'C')); //2,12
            figures.Add(new Pawn(new Position(0, 6), 'D')); //2,16
            // adding kingS
            figures.Add(new King(new Position(7, 3))); //9, 10
            figures.Add(new King(new Position(4, 4))); //9, 10

            GameBoard gameBoard = new GameBoard(figures);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestGameBoard_NoPawnsOnTheBoard()
        {            
            List<Figure> figures = new List<Figure>();                        
            figures.Add(new King(new Position(7, 3))); //9, 10

            GameBoard gameBoard = new GameBoard(figures);            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestGameBoard_FigurePositionOutOfTheBoard()
        {
            List<Figure> figures = new List<Figure>();
            figures.Add(new Pawn(new Position(1, 1), 'A')); // need to add some figures because we don`t want 
            figures.Add(new King(new Position(2, 2)));      // the exception to happen because of no pawns or king
            figures.Add(new Pawn(new Position(1000,1000),'B'));
            
            GameBoard gameBoard = new GameBoard(figures);
        }
    }
}
