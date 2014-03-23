using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerProject.PokerGame.PlayerClasses;
using System.Collections.Generic;
using System.Linq;

namespace PokerUnitTest
{
    [TestClass]
    public class PlayerPositionsTests
    {
        PlayerQueue testQueue;
        List<Player> testPlayers;
        PlayerPositions positions;

        [TestInitialize]
        public void SetUp()
        {
            positions = new PlayerPositions();

            testPlayers = new List<Player>();
            testPlayers.Add(new Player("Jack"));
            testPlayers.Add(new Player("Jill"));
            testPlayers.Add(new Player("Bob"));
            testPlayers.Add(new Player("Albert"));
            testPlayers.Add(new Player("Dennis"));
            testPlayers.Add(new Player("Barbara"));
            testPlayers.Add(new Player("Carla"));
            testQueue.AddPlayers(testPlayers);
        }

        [TestMethod]
        public void PlayerPositionSetGetDealerTest()
        {
            foreach (Player testPlayer in testPlayers)
            {
                positions.SetDealer(testPlayer);
                Player dealer = positions.GetDealer();
                Assert.AreEqual(testPlayer.Name, dealer.Name);
            }
        }

        [TestMethod]
        public void PlayerPositionSetGetSmallBlindTest()
        {
            foreach (Player testPlayer in testPlayers)
            {
                positions.SetSmallBlind(testPlayer);
                Player dealer = positions.GetSmallBlind();
                Assert.AreEqual(testPlayer.Name, dealer.Name);
            }
        }

        [TestMethod]
        public void PlayerPositionGetPlayerPositionTest()
        {
            Player dealer = testPlayers.ElementAt(3);
            Player smallBlind = testPlayers.ElementAt(4);
            Player bigBlind = testPlayers.ElementAt(5);

            positions.SetDealer(dealer);

            Assert.AreEqual("Dealer", positions.GetPlayerPosition(dealer));
            Assert.AreEqual("Small Blind", positions.GetPlayerPosition(smallBlind));
            Assert.AreEqual("Big Blind", positions.GetPlayerPosition(bigBlind));
        }
    }
}
