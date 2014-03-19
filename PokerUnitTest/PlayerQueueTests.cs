using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerProject.PokerGame.PlayerClasses;
using System.Collections.Generic;
using System.Linq;

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
            playerQueue = new PlayerQueue();

            testPlayers = new List<Player>();
            testPlayers.Add(new Player("Jack"));
            testPlayers.Add(new Player("Jill"));
            testPlayers.Add(new Player("Bob"));
            testPlayers.Add(new Player("Albert"));
            testPlayers.Add(new Player("Dennis"));
            testPlayers.Add(new Player("Barbara"));
            testPlayers.Add(new Player("Carla"));
        }

        [TestMethod]
        public void PlayerQueueAddPlayerTest()
        {
            Player jack = new Player("Jack");

            playerQueue.AddPlayer(jack);
            ICollection<Player> testedPlayerList = playerQueue.GetPlayers();

            Assert.AreEqual(1, testedPlayerList.Count);
            Assert.AreEqual(jack.Name, testedPlayerList.ElementAt(0).Name);
            Assert.AreNotSame(jack, testedPlayerList.ElementAt(0), "Players have the same reference");
        }

        [TestMethod]
        public void PlayerQueueAddPlayerListTest()
        {
            playerQueue.AddPlayers(testPlayers);
            ICollection<Player> testedPlayerList = playerQueue.GetPlayers();

            Assert.AreEqual(testPlayers.Count, testedPlayerList.Count);
            int index = 0;
            foreach (Player player in testPlayers)
            {
                Assert.AreEqual(player.Name, testedPlayerList.ElementAt(index).Name);
                Assert.AreNotSame(player, testedPlayerList.ElementAt(index), "Players have the same reference");
                index++;
            }
        }

        [TestMethod]
        public void PlayerQueueGetNextPlayerTest()
        {
            playerQueue.AddPlayers(testPlayers);

            Player nextPlayer;
            foreach (Player player in testPlayers)
            {
                nextPlayer = playerQueue.GetNextPlayer();
                Assert.AreEqual(player.Name, nextPlayer.Name);
            }
        }

        [TestMethod]
        public void PlayerQueueSetFirstPlayerOrderTest()
        {
            playerQueue.AddPlayers(testPlayers);

            Player nextPlayer;
            foreach (Player player in testPlayers)
            {
                nextPlayer = playerQueue.GetNextPlayer();
                Assert.AreEqual(player.Name, nextPlayer.Name);
            }

            int testIndex = 3;
            playerQueue.SetPlayerFirstInOrder(playerQueue.GetPlayers().ElementAt(testIndex));

            if (!playerQueue.HasNextPlayer())
            {
                Assert.Fail();
            }

            while(playerQueue.HasNextPlayer())
            {
                nextPlayer = playerQueue.GetNextPlayer();
                Assert.AreEqual(testPlayers.ElementAt(testIndex).Name, nextPlayer.Name);
                testIndex = (testIndex == testPlayers.Count - 1) ? 0 : testIndex + 1 ;
            }

            Assert.AreEqual(3, testIndex);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PlayerQueueSetFirstPlayerWrongTest()
        {
            playerQueue.AddPlayers(testPlayers);
            playerQueue.SetPlayerFirstInOrder(new Player("Jill"));
        }

        [TestMethod]
        public void PlayerQueueGetNextPlayerAfterPlayerTest()
        {
            playerQueue.AddPlayers(testPlayers);
            ICollection<Player> testPlayerList = playerQueue.GetPlayers();

            for (int testIndex = 0; testIndex < testPlayers.Count-1; testIndex++)
            {
                Player player = testPlayerList.ElementAt(testIndex);
                Player nextPlayer = playerQueue.GetNextPlayerAfterPlayer(player);

                Assert.AreEqual(testPlayers.ElementAt(testIndex + 1).Name, nextPlayer.Name);
            }
        }

        [TestMethod]
        public void PlayerQueueSetGetDealerTest()
        {
            playerQueue.AddPlayers(testPlayers);
            foreach (Player testPlayer in testPlayers)
            {
                Player nextPlayer = playerQueue.GetNextPlayer();
                playerQueue.SetDealer(nextPlayer);
                Player dealer = playerQueue.GetDealer();
                Assert.AreEqual(testPlayer.Name, dealer.Name);
            }
        }

        [TestMethod]
        public void PlayerQueueGetPlayerPositionTest()
        {
            playerQueue.AddPlayers(testPlayers);
            List<Player> queuePlayers = playerQueue.GetPlayers();

            Player dealer = queuePlayers.ElementAt(3);
            Player smallBlind = queuePlayers.ElementAt(4);
            Player bigBlind  = queuePlayers.ElementAt(5);

            playerQueue.SetDealer(dealer);

            Assert.AreEqual("Dealer", playerQueue.GetPlayerPosition(dealer));
            Assert.AreEqual("Small Blind", playerQueue.GetPlayerPosition(smallBlind));
            Assert.AreEqual("Big Blind", playerQueue.GetPlayerPosition(bigBlind));
        }

        [TestMethod]
        public void PlayerQueuePreFlopOrderTest()
        {
            playerQueue.AddPlayers(testPlayers);
            int testIndex = 2;
            Player dealer = playerQueue.GetPlayers().ElementAt(testIndex);
            playerQueue.SetDealer(dealer);

            playerQueue.SetBettingOrder();
            testIndex++;

            if (!playerQueue.HasNextPlayer())
            {
                Assert.Fail();
            }

            Player nextPlayer;

            while (playerQueue.HasNextPlayer())
            {
                nextPlayer = playerQueue.GetNextPlayer();
                Assert.AreEqual(testPlayers.ElementAt(testIndex).Name, nextPlayer.Name);
                testIndex = (testIndex == testPlayers.Count - 1) ? 0 : testIndex + 1;
            }

            Assert.AreEqual(3, testIndex);
        }

        [TestMethod]
        public void PlayerQueueSetNextHandTest()
        {
            playerQueue.AddPlayers(testPlayers);
            int testIndex = 2;
            Player firstDealer = playerQueue.GetPlayers().ElementAt(testIndex);
            playerQueue.SetDealer(firstDealer);

            playerQueue.SetNextHandOrder();

            testIndex++;
            testIndex++;

            if (!playerQueue.HasNextPlayer())
            {
                Assert.Fail();
            }

            Player nextPlayer;

            while (playerQueue.HasNextPlayer())
            {
                nextPlayer = playerQueue.GetNextPlayer();
                Assert.AreEqual(testPlayers.ElementAt(testIndex).Name, nextPlayer.Name);
                testIndex = (testIndex == testPlayers.Count - 1) ? 0 : testIndex + 1;
            }

            Assert.AreEqual(4, testIndex);
        }

        [TestMethod]
        public void PlayerQueueDeletePlayerTest()
        {
            playerQueue.AddPlayer(new Player("Jack"));
            playerQueue.AddPlayer(new Player("Bob"));

            List<Player> list = playerQueue.GetPlayers();

            Assert.AreEqual("Jack",list.ElementAt(0).Name);

            playerQueue.DeletePlayer(list.ElementAt(0));

            Assert.AreEqual("Bob", list.ElementAt(0).Name);
        }

        [TestMethod]
        public void PlayerQueueClearTest()
        {
            playerQueue.AddPlayers(testPlayers);

            playerQueue.Clear();

            List<Player> list = playerQueue.GetPlayers();
            Assert.AreEqual(0, list.Count);
        }

    }
}
