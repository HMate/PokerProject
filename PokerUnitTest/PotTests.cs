using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerProject;
using PokerProject.PokerGame;

namespace PokerUnitTest
{
    [TestClass]
    public class PotTests
    {
        Pot testPot;

        [TestInitialize]
        public void SetUp()
        {
            testPot = new Pot();
        }

        [TestMethod]
        public void PotPlaceBetTest()
        {
            testPot.PlaceBet(500);

            Assert.AreEqual(500, testPot.Size);
        }

        [TestMethod]
        public void PotPlaceMoreBetsTest()
        {
            place10Bets();

            Assert.AreEqual(550, testPot.Size);
        }

        [TestMethod]
        public void PotLargestBetTest()
        {
            place10Bets();

            Assert.AreEqual(100, testPot.LargestBet);
        }

        [TestMethod]
        public void PotEmptyTest()
        {
            place10Bets();
            testPot.Empty();

            Assert.AreEqual(0, testPot.Size);
            Assert.AreEqual(0, testPot.LargestBet);
        }

        private void place10Bets()
        {
            for (int index = 1; index <= 10; index++)
            {
                testPot.PlaceBet(index * 10);
            }
        }
    }
}
