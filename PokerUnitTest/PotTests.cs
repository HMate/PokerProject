using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerProject;
using PokerProject.PokerGame;
using PokerProject.PokerGame.PlayerClasses;

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
            Player testPlayer = new Player();
            testPot.PlaceBet(testPlayer, 500);

            Assert.AreEqual(500, testPot.Size);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PotWrongBetTest()
        {
            Player testPlayer = new Player();
            testPot.PlaceBet(testPlayer, 100);
            Player testPlayer2 = new Player();
            testPot.PlaceBet(testPlayer, 50);
            //Should throw error, as a player have to bet at least the amount of the previos bet, or bigger
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
        public void PotGetPlayerBetTest()
        {
            Player testPlayer = new Player();
            testPot.PlaceBet(testPlayer, 500);

            Assert.AreEqual(500, testPot.PlayerBetThisTurn(testPlayer));
        }

        [TestMethod]
        public void PotGetAmountToBeEligibleForPotTest()
        {
            place10Bets();

            Assert.AreEqual(100, testPot.AmountToBeEligibleForPot);
        }

        [TestMethod]
        public void PotGetAmountToCallTest()
        {
            place10Bets();
            Player testPlayer = new Player();
            testPot.PlaceBet(testPlayer, 500);

            Assert.AreEqual(0, testPot.GetAmountToCall(testPlayer));
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
                Player testPlayer = new Player();
                testPot.PlaceBet(testPlayer, index * 10);
            }
        }
    }
}
