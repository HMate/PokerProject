using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerProject.PokerGame;

namespace PokerUnitTest
{
    [TestClass]
    public class CardDeckTests
    {
        [TestMethod]
        public void TestDealCardFromDefaultDeck()
        {
            CardDeck deck = new CardDeck();
            PokerCard testCard = deck.DealCard();
            CardRank actualRank = testCard.Rank;
            CardSuite actualSuite = testCard.Suite;

            Assert.AreEqual(CardRank.Ace, actualRank);
            Assert.AreEqual(CardSuite.Hearts, actualSuite);
        }
    }
}
