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

        /*
         * Put test players in the queue.
         * */
        [TestInitialize]
        public void SetUp()
        {
            positions = Table.Instance.Positions;
            positions.ResetPositions();
            testQueue = Table.Instance.Players;
            testQueue.Clear();

            //Adding the players to the game
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

        /*
         * Tests for setting somebody as a dealer.
         * */
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

        /*
         * Test for asking for the players' positions
         * Only works well with at least 3 people in the game!
         * */
        [TestMethod]
        public void PlayerPositionGetPlayerPositionTest()
        {
            Player dealer = testQueue.GetNextPlayer();
            Player smallBlind = testQueue.GetNextPlayerAfterPlayer(dealer);
            Player bigBlind = testQueue.GetNextPlayerAfterPlayer(smallBlind);

            positions.SetDealer(dealer);

            Assert.AreEqual("Dealer", positions.GetPlayerPosition(dealer), "Dealers has wrong position");
            Assert.AreEqual("Small Blind", positions.GetPlayerPosition(smallBlind), "Small Blind has wrong position");
            Assert.AreEqual("Big Blind", positions.GetPlayerPosition(bigBlind), "Big Blind has wrong position");
        }

        /*
         * Test for getting positions if there are only 2 players in game.
         * */
        [TestMethod]
        public void PlayerPositionGetPlayerPositionForTwoPlayersTest()
        {
            SetQueueForTwoPlayers();

            Player dealer = testQueue.GetNextPlayer();
            Player smallBlind = testQueue.GetNextPlayerAfterPlayer(dealer);
            Player bigBlind = testQueue.GetNextPlayerAfterPlayer(smallBlind);

            positions.SetDealer(dealer);

            Assert.AreEqual("Dealer", positions.GetPlayerPosition(dealer), "Dealers has wrong position");
            Assert.AreEqual("Small Blind", positions.GetPlayerPosition(smallBlind), "Small Blind has wrong position");
            Assert.AreEqual("Dealer", positions.GetPlayerPosition(bigBlind), "Big Blind has wrong position");
            Assert.AreSame(dealer, bigBlind);
        }

        private void SetQueueForTwoPlayers()
        {
            testQueue.Clear();
            testQueue.AddPlayer(testPlayers[0]);
            testQueue.AddPlayer(testPlayers[1]);
        }

        /*
         * Test for setting the players' positions for the next hand
         * Only works well with at least 3 people in the game!
         * */
        [TestMethod]
        public void PlayerPositionSetNextHandPositionsTest()
        {
            Player dealer = testQueue.GetNextPlayer();
            Player smallBlind = testQueue.GetNextPlayerAfterPlayer(dealer);
            Player bigBlind = testQueue.GetNextPlayerAfterPlayer(smallBlind);
            Player nextBigBlind = testQueue.GetNextPlayerAfterPlayer(bigBlind);

            positions.SetDealer(dealer);
            Assert.AreEqual("Small Blind", positions.GetPlayerPosition(smallBlind));

            positions.SetNextHandPositions();
            Assert.AreEqual("Dealer", positions.GetPlayerPosition(smallBlind), "Dealers has wrong position");
            Assert.AreEqual("Small Blind", positions.GetPlayerPosition(bigBlind), "Small Blind has wrong position");
            Assert.AreEqual("Big Blind", positions.GetPlayerPosition(nextBigBlind), "Big Blind has wrong position");
        }

    }
}
