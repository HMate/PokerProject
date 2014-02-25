using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using PokerProject.PokerGame;

namespace PokerUnitTest
{
    [TestClass]
    public class PlayerTests
    {
        Player jack = new HumanPlayer();
        CardDeck deck = new CardDeck();

        [TestInitialize]
        public void SetUp()
        {
            jack = new HumanPlayer();
            deck = new CardDeck();
        }

        [TestMethod]
        public void PlayerNamingTest()
        {
            jack = new HumanPlayer("Jack");
            Assert.AreEqual("Jack", jack.Name);
        }

        [TestMethod]
        public void PlayerDrawCardsTest()
        {
            jack.DrawCard(deck);
            jack.DrawCard(deck);

            List<PokerCard> cards = jack.ShowCards();

            Assert.AreEqual(2, cards.Count);
            Assert.AreNotEqual(cards.ElementAt(0)  , cards.ElementAt(1));
        }

        [TestMethod]
        public void PlayersGetDifferentCardsTest()
        {
            Player ben = new HumanPlayer();
            jack.DrawCard(deck);
            jack.DrawCard(deck);
            ben.DrawCard(deck);
            ben.DrawCard(deck);

            List<PokerCard> bensCards = ben.ShowCards();
            List<PokerCard> jacksCards = jack.ShowCards();

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

            List<PokerCard> cards = jack.ShowCards();

            Assert.AreEqual(0, cards.Count);
        }

        [TestMethod]
        public void PlayerChipCountTest()
        {
            jack.ChipCount = 1000;
            jack.GiveChips(500);
            Assert.AreEqual(1500, jack.ChipCount);
        }

        [TestMethod]
        public void PlayHasNoChipsTest()
        {
            Assert.AreEqual(0, jack.ChipCount);
        }

        [TestMethod]
        public void PlayerTakeAwayChipsTest()
        {
            jack.GiveChips(1000);
            jack.TakeChips(300);
            Assert.AreEqual(700, jack.ChipCount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PlayerTakeAwayTooMuchChipsTest()
        {
            jack.GiveChips(500);
            jack.TakeChips(700);
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
            jack.GiveChips(-400);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PlayerTakeAwayNegativeChipsTest()
        {
            jack.TakeChips(-400);
        }
    }
}
