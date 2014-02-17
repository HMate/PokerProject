using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
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
            HashSet<PokerCard> dealedCards = new HashSet<PokerCard>();
            for (int cardIndex = 0; cardIndex < 52; cardIndex++)
            {
                PokerCard testCard = deck.DealOneCard();
                bool success = dealedCards.Add(testCard);
                if (!success) Assert.Fail("Couldn't add card to the set at index {0}", cardIndex);
            }
            Assert.IsTrue(true);
        }

    }
}
