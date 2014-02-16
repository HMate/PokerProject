using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerProject.PokerGame;

namespace PokerUnitTest
{
    [TestClass]
    public class CardDeckTests
    {
        [TestMethod]
        public void DealOneCardFromDefaultDeckTest()
        {
            CardDeck deck = new CardDeck();
            for (int i = 0; i < 26; i++)
            {
                PokerCard testCard = deck.DealOneCard();
                PokerCard testCard2 = deck.DealOneCard();
                Assert.AreNotEqual(testCard2, testCard,
                    "results: \ncard1 Rank:{0}, Suite:{1};\n card2 Rank:{2}, Suite:{3}",
                    testCard.Rank, testCard.Suite, testCard2.Rank, testCard2.Suite);
            }
        }
    }
}
