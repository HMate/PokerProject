using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerProject.PokerGame;

namespace PokerUnitTest
{
    [TestClass]
    public class PokerCardTests
    {
        [TestMethod]
        public void CreatePokerCard()
        {
            PokerCard card = new PokerCard(CardRank.Ace, CardSuite.Spades);
            PokerCard card2 = new PokerCard();
            PokerCard card3 = new PokerCard(5, 3);
            Assert.IsNotNull(card);
            Assert.IsNotNull(card2);
            Assert.IsNotNull(card3);
        }

        [TestMethod]
        public void PokerCardGetterTest()
        {
            PokerCard card = new PokerCard(CardRank.King, CardSuite.Clubs);
            CardSuite testedSuite = card.Suite;
            CardRank testedRank = card.Rank;

            Assert.AreEqual(CardRank.King, testedRank );
            Assert.AreEqual(CardSuite.Clubs, testedSuite);

            PokerCard card2 = new PokerCard(0, 0);
            testedSuite = card2.Suite;
            testedRank = card2.Rank;

            Assert.AreEqual(CardRank.Ace, testedRank);
            Assert.AreEqual(CardSuite.Hearts, testedSuite);
        }

        [TestMethod]
        public void testTwoCardsEqual()
        {
            PokerCard card1 = new PokerCard();
            PokerCard card2 = new PokerCard();

            Assert.AreEqual(card1, card2);
        }

        [TestMethod]
        public void testTwoCardsDontEqual()
        {
            PokerCard card1 = new PokerCard(CardRank.Five, CardSuite.Diamonds);
            PokerCard card2 = new PokerCard(CardRank.Ten, CardSuite.Diamonds);

            Assert.AreNotEqual(card1, card2);
        }

        [TestMethod]
        public void testCardsHaveSameRank()
        {
            PokerCard card1 = new PokerCard(CardRank.Ten, CardSuite.Diamonds);
            PokerCard card2 = new PokerCard(CardRank.Ten, CardSuite.Spades);
            PokerCard card3 = new PokerCard(CardRank.Five, CardSuite.Diamonds);

            Assert.IsTrue(card1.HasSameRank(card2));
            Assert.IsFalse(card1.HasSameRank(card3));
        }

        [TestMethod]
        public void testCardsHaveSameSuite()
        {
            PokerCard card1 = new PokerCard(CardRank.Ten, CardSuite.Diamonds);
            PokerCard card2 = new PokerCard(CardRank.Ten, CardSuite.Spades);
            PokerCard card3 = new PokerCard(CardRank.Five, CardSuite.Diamonds);

            Assert.IsTrue(card1.HasSameSuite(card3));
            Assert.IsFalse(card1.HasSameSuite(card2));
        }
    }
}
