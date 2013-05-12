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

        // from Subo
        // [TestMethod]
        //public void TestScenarioFigurite() // tuka testvame carq da pe4eli
        //{
        //    string moves = string.Format("kul bdr kul bdl kur adr kur bdl kur bdl kul ddl kul", Environment.NewLine);

        //    using (StringWriter stringWriter = new StringWriter())
        //    {
        //        Console.SetOut(stringWriter);

        //        using (StringReader stringReader = new StringReader(moves))
        //        {
        //            Console.SetIn(stringReader);

        //            Engine engine = new Engine(); //tuka s figurite

        //            engine.Run();

        //            string[] outputLines = stringWriter.ToString().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

        //            string expected = "King wins in 7 turns!";
        //            string actual = outputLines[outputLines.Length - 1];

        //            Assert.AreEqual(expected, actual);
        //        }
        //    }
        //}
    }
}
