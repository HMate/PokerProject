using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerProject.PokerGame;
using PokerProject.PokerGame.CardClasses;
using PokerProject.PokerGame.PlayerClasses;
using System.Collections.Generic;
using System.Linq;

namespace PokerUnitTest
{
    //TODO
    //Több int 8 játékos hozzáadása

    [TestClass]
    public class GameTests
    {
        Game assets;

        [TestInitialize]
        public void SetUp()
        {
            assets = new Game();
        }

        [TestMethod]
        public void GameCommunityCardsTest()
        {
            CardList cardList = createCards();

            assets.SetCommunityCards(cardList);

            CardList communityCards = assets.ShowCommunityCards();
            for (int index = 0; index < 52; index++)
            {
                Assert.AreEqual(cardList.ElementAt(index), communityCards.ElementAt(index), "Different cards! reference card:{0}, {1} /nlist card: {2}, {3}"
                    , cardList.ElementAt(index).Rank, cardList.ElementAt(index).Suite, communityCards.ElementAt(index).Rank, communityCards.ElementAt(index).Suite);

                Assert.AreNotSame(cardList.ElementAt(index), communityCards.ElementAt(index), "Cards have the same reference!");
            }
        }

        private CardList createCards()
        {
            CardList cardList = new CardList();
            int index = 0;
            foreach (CardSuite suiteIndex in (CardSuite[])Enum.GetValues(typeof(CardSuite)))
            {
                foreach (CardRank rankIndex in (CardRank[])Enum.GetValues(typeof(CardRank)))
                {
                    PokerCard referenceCard = new PokerCard(rankIndex, suiteIndex);
                    cardList.Add(referenceCard);
                    index++;
                }
            }
            return cardList;
        }

        [TestMethod]
        public void GameConstructorWithPlayersTest()
        {
            Player jack = new HumanPlayer("Jack");
            Player jill = new HumanPlayer("Jill");
            List<Player> playerList = new List<Player>();
            playerList.Add(jack);
            playerList.Add(jill);

            assets = new Game(playerList);

            List<Player> testedPlayerList = assets.GetPlayers();
            Assert.AreNotSame(playerList, testedPlayerList);
            Assert.AreEqual(jack.Name, testedPlayerList.ElementAt(0).Name);
            Assert.AreEqual(jill.Name, testedPlayerList.ElementAt(1).Name);
            Assert.AreNotSame(jack, testedPlayerList.ElementAt(0), "Players have the same reference");
            Assert.AreNotSame(jill, testedPlayerList.ElementAt(1), "Players have the same reference");
        }

        [TestMethod]
        public void GameAddingPlayersTest()
        {
            Player jack = new HumanPlayer("Jack");
            Player jill = new HumanPlayer("Jill");
            assets.AddPlayer(jack);
            assets.AddPlayer(jill);

            List<Player> testedPlayerList = assets.GetPlayers();

            Assert.AreEqual(jack.Name, testedPlayerList.ElementAt(0).Name);
            Assert.AreSame(jill.Name, testedPlayerList.ElementAt(1).Name);
            Assert.AreNotSame(jack, testedPlayerList.ElementAt(0));
            Assert.AreNotSame(jill, testedPlayerList.ElementAt(1));
        }

        [TestMethod]
        public void GameAddingPlayersInListTest()
        {
            Player jack = new HumanPlayer("Jack");
            Player jill = new HumanPlayer("Jill");
            List<Player> playerList = new List<Player>();
            playerList.Add(jack);
            playerList.Add(jill);

            assets.AddPlayers(playerList);

            List<Player> testedPlayerList = assets.GetPlayers();
            Assert.AreNotSame(playerList, testedPlayerList);
            Assert.AreEqual(jack.Name, testedPlayerList.ElementAt(0).Name);
            Assert.AreEqual(jill.Name, testedPlayerList.ElementAt(1).Name);
            Assert.AreNotSame(jack, testedPlayerList.ElementAt(0), "Players have the same reference");
            Assert.AreNotSame(jill, testedPlayerList.ElementAt(1), "Players have the same reference");
        }
    }
}
