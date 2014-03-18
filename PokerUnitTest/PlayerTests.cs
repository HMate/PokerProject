using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using PokerProject.PokerGame.CardClasses;
using PokerProject.PokerGame.PlayerClasses;

namespace PokerUnitTest
{
    [TestClass]
    public class PlayerTests
    {
        Player jack = new Player();
        CardDeck deck = new CardDeck();

        [TestInitialize]
        public void SetUp()
        {
            jack = new Player("Jack");
            deck = new CardDeck();
        }

        [TestMethod]
        public void PlayerNamingTest()
        {
            string name = "Jack";

            Assert.AreEqual(name, jack.Name);
        }

        [TestMethod]
        public void PlayerCopyConstructorTest()
        {
           Player jackCopy = new Player(jack);
           CardList jacksCardList = jack.ShowCards();
           CardList jackCopysCardList = jackCopy.ShowCards();

           Assert.AreEqual(jack.Name, jackCopy.Name);
           Assert.AreEqual(jack.ChipCount, jackCopy.ChipCount);
           Assert.AreNotSame(jack, jackCopy);
        }

        [TestMethod]
        public void PlayerHumanCopyTest()
        {
            Player otherPlayer = jack.Clone();

            Assert.AreEqual(jack.Name, otherPlayer.Name);
            Assert.AreNotSame(jack, otherPlayer);
        }

        [TestMethod]
        public void PlayerDrawCardsTest()
        {
            jack.DrawCard(deck);
            jack.DrawCard(deck);

            CardList cards = jack.ShowCards();

            Assert.AreEqual(2, cards.Count);
            Assert.AreNotEqual(cards.ElementAt(0)  , cards.ElementAt(1));
        }

        [TestMethod]
        public void PlayerDrawPredefinedCardsTest()
        {
            PokerCard card1 = new PokerCard(CardRank.Five, CardSuite.Hearts);
            PokerCard card2 = new PokerCard(CardRank.Ten, CardSuite.Clubs);
            jack.DrawCard(card1);
            jack.DrawCard(card2);
            CardList cardList = jack.ShowCards();

            Assert.AreEqual(cardList.ElementAt(0), card1);
            Assert.AreEqual(cardList.ElementAt(1), card2);
        }

        [TestMethod]
        public void PlayersGetDifferentCardsTest()
        {
            Player ben = new Player();
            jack.DrawCard(deck);
            jack.DrawCard(deck);
            ben.DrawCard(deck);
            ben.DrawCard(deck);

            CardList bensCards = ben.ShowCards();
            CardList jacksCards = jack.ShowCards();

            Assert.AreNotEqual(bensCards.ElementAt(0), jacksCards.ElementAt(0));
            Assert.AreNotEqual(bensCards.ElementAt(0), jacksCards.ElementAt(1));
            Assert.AreNotEqual(bensCards.ElementAt(1), jacksCards.ElementAt(0));
            Assert.AreNotEqual(bensCards.ElementAt(1), jacksCards.ElementAt(1));
        }

        [TestMethod]
        public void PlayerFoldCardsTest()
        {
            jack.DrawCard(deck);
            jack.DrawCard(deck);
            jack.FoldCards();

            CardList cards = jack.ShowCards();

            Assert.AreEqual(0, cards.Count);
        }

        [TestMethod]
        public void PlayerChipCountTest()
        {
            jack.ChipCount = 1000;
            jack.IncreaseChipCount(500);
            Assert.AreEqual(1500, jack.ChipCount);
        }

        [TestMethod]
        public void PlayerHasNoChipsTest()
        {
            Assert.AreEqual(0, jack.ChipCount);
        }

        [TestMethod]
        public void PlayerTakeAwayChipsTest()
        {
            jack.IncreaseChipCount(1000);
            jack.DecreaseChipCount(300);
            Assert.AreEqual(700, jack.ChipCount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PlayerTakeAwayTooMuchChipsTest()
        {
            jack.IncreaseChipCount(500);
            jack.DecreaseChipCount(700);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PlayerChipCountGetsNegativeChipsTest()
        {
            jack.ChipCount = -400;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PlayerGetsNegativeChipsTest()
        {
            jack.IncreaseChipCount(-400);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PlayerTakeAwayNegativeChipsTest()
        {
            jack.DecreaseChipCount(-400);
        }
    }
}
