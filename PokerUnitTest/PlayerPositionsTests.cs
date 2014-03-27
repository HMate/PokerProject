using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerProject.PokerGame.PlayerClasses;
using System.Collections.Generic;
using System.Linq;
using PokerProject.PokerGame;

namespace PokerUnitTest
{
    [TestClass]
    public class PlayerPositionsTests
    {
        PlayerQueue testQueue;
        List<Player> testPlayers;
        PlayerPositions positions;

        [TestInitialize]
        /*
         * Put test players in the queue.
         * */
        public void SetUp()
        {
            positions = Table.Instance.Positions;
            positions.ResetPositions();
            testQueue = Table.Instance.Players;
            testQueue.Clear();

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
            while (testQueue.HasNextPlayer())
            {
                Player testPlayer = testQueue.GetNextPlayer();
                positions.SetDealer(testPlayer);
                Player dealer = positions.GetDealer();
                Assert.AreEqual(testPlayer.Name, dealer.Name);
            }
        }

        [TestMethod]
        public void PlayerPositionSetGetSmallBlindTest()
        {
            while (testQueue.HasNextPlayer())
            {
                Player testPlayer = testQueue.GetNextPlayer();
                positions.SetSmallBlind(testPlayer);
                Player dealer = positions.GetSmallBlind();
                Assert.AreEqual(testPlayer.Name, dealer.Name);
            }
        }

        [TestMethod]
        /*
         * Test for asking for the players' positions
         * Only works well with at least 3 people in the game!
         * */
        public void PlayerPositionGetPlayerPositionTest()
        {
            Player dealer = testQueue.GetNextPlayer();
            Player smallBlind = testQueue.GetNextPlayerAfterPlayer(dealer);
            Player bigBlind = testQueue.GetNextPlayerAfterPlayer(smallBlind);

            positions.SetDealer(dealer);

            Assert.AreEqual("Dealer", positions.GetPlayerPosition(dealer));
            Assert.AreEqual("Small Blind", positions.GetPlayerPosition(smallBlind));
            Assert.AreEqual("Big Blind", positions.GetPlayerPosition(bigBlind));
        }

        [TestMethod]
        /*
         * Test for setting the players' positions for the next hand
         * Only works well with at least 3 people in the game!
         * */
        public void PlayerPositionSetNextHandPositionsTest()
        {
            Player dealer = testQueue.GetNextPlayer();
            Player smallBlind = testQueue.GetNextPlayerAfterPlayer(dealer);
            Player bigBlind = testQueue.GetNextPlayerAfterPlayer(smallBlind);
            Player nextBigBlind = testQueue.GetNextPlayerAfterPlayer(bigBlind);

            positions.SetDealer(dealer);
            Assert.AreEqual("Small Blind", positions.GetPlayerPosition(smallBlind));

            positions.SetNextHandPositions();
            Assert.AreEqual("Dealer", positions.GetPlayerPosition(smallBlind));
            Assert.AreEqual("Small Blind", positions.GetPlayerPosition(bigBlind));
            Assert.AreEqual("Big Blind", positions.GetPlayerPosition(nextBigBlind));
        }
    }
}
