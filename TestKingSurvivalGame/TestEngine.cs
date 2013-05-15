﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KingSurvivalGame;
using System.IO;
using System.Collections.Generic;

namespace TestKingSurvivalGame
{
    [TestClass]
    public class TestEngine
    {
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

        public void TestProcessASide()
        {
            string num = "KDL";
            Console.SetIn(new StringReader(num));

            List<Figure> figures = new List<Figure>();
            figures.Add(new Pawn(new Position(0, 0), 'A'));
            figures.Add(new Pawn(new Position(0, 2), 'B'));
            figures.Add(new Pawn(new Position(0, 4), 'C'));
            figures.Add(new Pawn(new Position(0, 6), 'D'));
            figures.Add(new King(new Position(7, 3)));
            GameBoard gameBoard = new GameBoard(figures);

            Engine currentEngine = new Engine(gameBoard, figures);
            currentEngine.ProcessASide("King");


            Assert.IsTrue(currentEngine.IsValidCommand, "Well processed");

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
