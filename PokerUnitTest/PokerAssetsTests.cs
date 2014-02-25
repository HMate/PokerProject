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
    public class PokerAssetsTests
    {
        PokerAssets assets;

        [TestInitialize]
        public void SetUp()
        {
            assets = new PokerAssets();
        }

        [TestMethod]
        public void PokerAssetsCommunityCardsTest()
        {
            List<PokerCard> cardList = createCards(5);

            assets.SetCommunityCards(cardList);

            List<PokerCard> communityCards = assets.ShowCommunityCards();
            Assert.AreEqual(cardList.ElementAt(0), communityCards.ElementAt(0));
            Assert.AreEqual(cardList.ElementAt(1), communityCards.ElementAt(1));
            Assert.AreEqual(cardList.ElementAt(2), communityCards.ElementAt(2));
            Assert.AreEqual(cardList.ElementAt(3), communityCards.ElementAt(3));
            Assert.AreEqual(cardList.ElementAt(4), communityCards.ElementAt(4));
        }

        [TestMethod]
        public void PokerAssetsCommunityCardsParameterTest()
        {
            List<PokerCard> cardList = createCards(5);

            assets.SetCommunityCards(cardList);
            cardList.Add(new PokerCard());

            List<PokerCard> communityCards = assets.ShowCommunityCards();
            Assert.AreNotEqual(cardList, communityCards);
        }

        private List<PokerCard> createCards(int number)
        {
            List<PokerCard> cardList = new List<PokerCard>();
            for (int i = 0; i < number; ++i)
            {
                PokerCard card = new PokerCard();
                cardList.Add(card);
            }
            return cardList;
        }

        [TestMethod]
        public void PokerAssetsConstructorWithPlayersTest()
        {
            Player jack = new HumanPlayer("Jack");
            Player jill = new HumanPlayer("Jill");
            List<Player> playerList = new List<Player>();
            playerList.Add(jack);
            playerList.Add(jill);

            assets = new PokerAssets(playerList);

            List<Player> testedPlayerList = assets.GetPlayers();
            Assert.AreNotSame(playerList, testedPlayerList);
            Assert.AreEqual(jack.Name, testedPlayerList.ElementAt(0).Name);
            Assert.AreEqual(jill.Name, testedPlayerList.ElementAt(1).Name);
            Assert.AreNotSame(jack, testedPlayerList.ElementAt(0), "Players have the same reference");
            Assert.AreNotSame(jill, testedPlayerList.ElementAt(1), "Players have the same reference");
        }

        [TestMethod]
        public void PokerAssetsAddingPlayersTest()
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
        public void PokerAssetsAddingPlayersInListTest()
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
