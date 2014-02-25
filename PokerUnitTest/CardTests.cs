using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerProject.PokerGame.CardClasses;

namespace PokerUnitTest
{
    [TestClass]
    public class CardTests
    {
        [TestMethod]
        public void CardCreateTest()
        {
            PokerCard card = new PokerCard(CardRank.Ace, CardSuite.Spades);
            PokerCard card2 = new PokerCard();
            Assert.IsNotNull(card);
            Assert.IsNotNull(card2);
        }

        [TestMethod]
        public void CardGetterTest()
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
        public void CardsEqualTest()
        {
            PokerCard card1 = new PokerCard();
            PokerCard card2 = new PokerCard();

            Assert.AreEqual(card1, card2);
        }

        [TestMethod]
        public void CardsDontEqualTest()
        {
            PokerCard card1 = new PokerCard(CardRank.Five, CardSuite.Diamonds);
            PokerCard card2 = new PokerCard(CardRank.Ten, CardSuite.Diamonds);

            Assert.AreNotEqual(card1, card2);
        }

        [TestMethod]
        public void CardsHaveSameRankTest()
        {
            PokerCard card1 = new PokerCard(CardRank.Ten, CardSuite.Diamonds);
            PokerCard card2 = new PokerCard(CardRank.Ten, CardSuite.Spades);
            PokerCard card3 = new PokerCard(CardRank.Five, CardSuite.Diamonds);

            Assert.IsTrue(card1.HasSameRank(card2));
            Assert.IsFalse(card1.HasSameRank(card3));
        }

        [TestMethod]
        public void CardsHaveSameSuiteTest()
        {
            PokerCard card1 = new PokerCard(CardRank.Ten, CardSuite.Diamonds);
            PokerCard card2 = new PokerCard(CardRank.Ten, CardSuite.Spades);
            PokerCard card3 = new PokerCard(CardRank.Five, CardSuite.Diamonds);

            Assert.IsTrue(card1.HasSameSuite(card3));
            Assert.IsFalse(card1.HasSameSuite(card2));
        }

        [TestMethod]
        public void CardRankSuiteParameterTest()
        {
            CardRank rank = CardRank.Ace;
            CardSuite suite = CardSuite.Clubs;

            PokerCard card = new PokerCard(rank, suite);
            rank = CardRank.King;
            suite = CardSuite.Diamonds;

            Assert.AreEqual(CardRank.Ace, card.Rank);
            Assert.AreEqual(CardSuite.Clubs, card.Suite);
            Assert.AreNotEqual(CardRank.King, card.Rank);
            Assert.AreNotEqual(CardSuite.Diamonds, card.Suite);


        }
    }
}
