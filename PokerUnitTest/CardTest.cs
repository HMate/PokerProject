using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerProject.PokerGame;

namespace PokerUnitTest
{
    [TestClass]
    public class PokerCardTest
    {
        [TestMethod]
        public void CreatePokerCard()
        {
            PokerCard card = new PokerCard(CardRank.Ace, CardSuite.Spades);
            PokerCard card2 = new PokerCard();
            Assert.IsNotNull(card);
            Assert.IsNotNull(card2);
        }

        [TestMethod]
        public void PokerCardGetterTest()
        {
            PokerCard card = new PokerCard(CardRank.King, CardSuite.Clubs);
            CardSuite testedSuite = card.CardSuite;
            CardRank testedRank = card.CardRank;

            Assert.AreEqual(testedRank, CardRank.King);
            Assert.AreEqual(testedSuite, CardSuite.Clubs);
        }



    }
}
