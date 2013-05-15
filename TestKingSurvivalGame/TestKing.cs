using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KingSurvivalGame;

namespace TestKingSurvivalGame
{
    [TestClass]
    public class TestKing
    {
        //TODO: King moves (only up, UR, UL,down...)
        [TestMethod]
        public void TestKing_Position()
        {
            for (int i = 0; i < 100; i++)
            {
                Random random = new Random();
                int row = random.Next(0, 7);
                int col = random.Next(0, 7);
                Position position = new Position(row, col);
                King king = new King(position);
                Assert.AreEqual<Position>(king.Position, position, "King`s position is not working correctly");
            }
        }       
    }
}
