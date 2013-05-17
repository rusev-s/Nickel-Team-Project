using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KingSurvivalGame;

namespace TestKingSurvivalGame
{
    [TestClass]
    public class TestPawn
    {
        //TODO: Pawn moves (only up, UR, UL,down...)        

        [TestMethod]
        public void TestPawn_SymbolRepresentationValidLetter_A()
        {
            Pawn pawn = new Pawn(new Position(3, 0), 'A');
            char actualSymbol = pawn.SymbolRepresentation;
            Assert.AreEqual('A', actualSymbol, "Pawn symbol representation is not working correctly");
        }

        [TestMethod]
        public void TestPawn_SymbolRepresentationValidLetter_Z()
        {
            Pawn pawn = new Pawn(new Position(3, 0), 'Z');
            char actualSymbol = pawn.SymbolRepresentation;
            Assert.AreEqual('Z', actualSymbol, "Pawn symbol representation is not working correctly");
        }

        [TestMethod]
        public void TestPawn_SymbolRepresentationValidLetter_a()
        {
            Pawn pawn = new Pawn(new Position(3, 0), 'a');
            char actualSymbol = pawn.SymbolRepresentation;
            Assert.AreEqual('a', actualSymbol, "Pawn symbol representation is not working correctly");
        }

        [TestMethod]        
        public void TestPawn_SymbolRepresentationValidLetter_z()
        {
            Pawn pawn = new Pawn(new Position(3, 0), 'z');
            char actualSymbol = pawn.SymbolRepresentation;
            Assert.AreEqual('z', actualSymbol, "Pawn symbol representation is not working correctly");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestPawn_SymbolRepresentationInvalidSymbol_Space()
        {
            //TODO: to finish that method
            Pawn pawn = new Pawn(new Position(3, 0), ' ');            
        }
    }
}
