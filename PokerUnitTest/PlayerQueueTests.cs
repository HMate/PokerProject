using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerProject.PokerGame.PlayerClasses;
using System.Collections.Generic;
using System.Linq;
using PokerProject.PokerGame;

namespace PokerUnitTest
{
    [TestClass]
    public class PlayerQueueTests
    {
        PlayerQueue playerQueue;
        List<Player> testPlayers;

        [TestInitialize]
        public void SetUp()
        {
            playerQueue = Table.Instance.Players;
            playerQueue.Clear();

            //I make a list of players with which i can initialize the queue and use them for testing.
            testPlayers = new List<Player>();
            testPlayers.Add(new Player("Jack"));
            testPlayers.Add(new Player("Jill"));
            testPlayers.Add(new Player("Bob"));
            testPlayers.Add(new Player("Albert"));
            testPlayers.Add(new Player("Dennis"));
            testPlayers.Add(new Player("Barbara"));
            testPlayers.Add(new Player("Carla"));

            //I add these players to the queue
            playerQueue.AddPlayers(testPlayers);
        }

        [TestMethod]
        public void PlayerQueueClearTest()
        {
            playerQueue.Clear();

            Assert.AreEqual(0, playerQueue.Count());
        }

        [TestMethod]
        /*
         * Test for adding a single person to the player queue.
         * */
        public void PlayerQueueAddPlayerTest()
        {
            playerQueue.Clear();
            Player jack = new Player("Jack");

            playerQueue.AddPlayer(jack);
            ICollection<Player> testedPlayerList = playerQueue.GetPlayersList();

            Assert.AreEqual(1, testedPlayerList.Count);
            Assert.AreEqual(jack.Name, testedPlayerList.ElementAt(0).Name);
        }

        [TestMethod]
        /*
         * Tests that the players in the queue are matchin the players that we put into the queue. 
         * */
        public void PlayerQueueAddPlayerListTest()
        {
            ICollection<Player> testedPlayerList = playerQueue.GetPlayersList();

            Assert.AreEqual(testPlayers.Count, testedPlayerList.Count);
            int index = 0;
            foreach (Player player in testPlayers)
            {
                Assert.AreEqual(player.Name, testedPlayerList.ElementAt(index).Name);
                index++;
            }
        }

        /*
         * Tests getNextPlayer method for playerQueue
         * */
        [TestMethod]
        public void PlayerQueueGetNextPlayerTest()
        {
            foreach (Player player in testPlayers)
            {
                Player nextPlayer = playerQueue.GetNextPlayer();
                Assert.AreEqual(player.Name, nextPlayer.Name);
            }
        }

        /*
         * If you try to set a player the first in the who isn't in the queue,
         * the queue will throw an exception.
         * */
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PlayerQueueSetFirstPlayerWrongTest()
        {
            playerQueue.SetPlayerFirstInOrder(new Player("Jill"));
        }

        [TestMethod]
        public void PlayerQueueGetNextPlayerAfterPlayerTest()
        {
            ICollection<Player> testPlayerList = playerQueue.GetPlayersList();

            for (int testIndex = 0; testIndex < testPlayers.Count - 1; testIndex++)
            {
                Player player = testPlayerList.ElementAt(testIndex);
                Player nextPlayer = playerQueue.GetNextPlayerAfterPlayer(player);

                Assert.AreEqual(testPlayers.ElementAt(testIndex + 1).Name, nextPlayer.Name);
            }
        }

        [TestMethod]
        /*
         * Test for setting the first player in the player queue
         * with the SetPlayerFirstInOrder method
         * After setting the first player, I iterate through the queue to check 
         * that every player was in the queue and that the indexes are correct.
         * */
        public void PlayerQueueSetFirstPlayerOrderTest()
        {
            const int beginningIndex = 3;
            int testIndex = beginningIndex;

            playerQueue.SetPlayerFirstInOrder(playerQueue.GetPlayersList().ElementAt(testIndex));
            int finalIndex = IterateThroughQueueAndAssertIndexes(testIndex);

            Assert.AreEqual(beginningIndex, finalIndex);
        }


        [TestMethod]
        /*
         * Test for setting the first player to the first bettor in the player queue
         * with the SetBettingOrder method
         * After setting the first player, I iterate through the queue to check 
         * that every player was in the queue and that the indexes are correct.
         * */
        public void PlayerQueuePreFlopOrderTest()
        {
            const int beginningIndex = 2;
            int testIndex = beginningIndex;
            SetDealerByIndex(testIndex);

            playerQueue.SetBettingOrder();
            //During betting the first player in the queue
            //will be the player next to the dealer
            testIndex++;

            int finalIndex = IterateThroughQueueAndAssertIndexes(testIndex);

            Assert.AreEqual(beginningIndex + 1, finalIndex);
        }

        /*
         * Sets the dealer for the player thats in the queue at the given index
         * */
        private void SetDealerByIndex(int testIndex)
        {
            Player firstDealer = playerQueue.GetPlayersList().ElementAt(testIndex);
            Table.Instance.Positions.ResetPositions();
            Table.Instance.Positions.SetDealer(firstDealer);
        }

        /*
         * Check if the queue has players int it.
         * After that iterates through the queue and checks that the indexes are matching.
         * The first player in the queue have to be the player with the given testIndex
         * */
        private int IterateThroughQueueAndAssertIndexes(int testIndex)
        {
            AssertPlayerQueueHasNextPlayer();

            Player nextPlayer;
            while (playerQueue.HasNextPlayer())
            {
                nextPlayer = playerQueue.GetNextPlayer();
                Assert.AreEqual(testPlayers.ElementAt(testIndex).Name, nextPlayer.Name);
                testIndex = (testIndex == testPlayers.Count - 1) ? 0 : testIndex + 1;
            }
            return testIndex;
        }

        private void AssertPlayerQueueHasNextPlayer()
        {
            if (!playerQueue.HasNextPlayer())
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        /*
         * Test for deleting players from the queue
         * */
        public void PlayerQueueDeletePlayerTest()
        {
            playerQueue.Clear();
            playerQueue.AddPlayer(new Player("Jack"));
            playerQueue.AddPlayer(new Player("Bob"));

            List<Player> list = playerQueue.GetPlayersList();

            Assert.AreEqual("Jack",list.ElementAt(0).Name);

            playerQueue.DeletePlayer(list.ElementAt(0));

            Assert.AreEqual("Bob", list.ElementAt(0).Name);
        }
    }
}
