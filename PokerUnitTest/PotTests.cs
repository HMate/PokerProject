using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerProject;
using PokerProject.PokerGame;

namespace PokerUnitTest
{
    [TestClass]
    public class PotTests
    {
        [TestMethod]
        public void PotPlaceBetTest()
        {
            Pot testPot = new Pot();
            testPot.PlaceBet(500);

            Assert.AreEqual(500, testPot.Size);
        }

        [TestMethod]
        public void PotPlaceMoreBetsTest()
        {
            Pot testPot = new Pot();
            for (int index = 1; index <= 10; index++)
            {
                testPot.PlaceBet(index*10);
            }

            Assert.AreEqual(550, testPot.Size);
        }
    }
}
