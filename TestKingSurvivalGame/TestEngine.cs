using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KingSurvivalGame;
namespace TestKingSurvivalGame
{
    [TestClass]
    public class TestEngine
    {
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestEngineConstructorSetNullValue_ThrowsException()
        {
            Engine currentEngine = new Engine(null,null);
        }
    }
}
