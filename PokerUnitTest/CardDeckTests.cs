using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using PokerProject.PokerGame.CardClasses;

namespace PokerUnitTest
{
    [TestClass]
    public class CardDeckTests
    {
        CardDeck deck;
        HashSet<PokerCard> uniqueCards;

        [TestInitialize]
        public void SetUp()
        {
            deck = new CardDeck();
            uniqueCards = new HashSet<PokerCard>();
        }

        [TestMethod]
        public void DealOneCardMethodTest()
        {
            Deal52Cards();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void DealMoreCardsThan52CardsTest()
        {
            try
            {
                Deal52Cards();
                ClearUniqueCardsSet();
                Deal52Cards();
            }
            catch(CardDeckEmptyException)
            {
                return;
            }
            Assert.Fail();
        }

        [TestMethod]
        public void DeckRecreateTest()
        {
            Deal52Cards();
            ClearUniqueCardsSet();
            deck.createNewPokerDeck();
            Deal52Cards();
        }

        [TestMethod]
        public void DeckSizeRemainingTest()
        {
            Deal52Cards();
            Assert.AreEqual(0, deck.GetDeckSize());
        }

        private void Deal52Cards()
        {
            for (int cardIndex = 0; cardIndex < CardDeck.defaultDeckSize; cardIndex++)
            {
                PokerCard testCard = deck.DealOneCard();
                bool success = uniqueCards.Add(testCard);

                if (!success)
                {
                    Assert.Fail("Couldn't add card ({0} of {1}) to the set at index {2}",  testCard.Rank, testCard.Suite, cardIndex);
                }
            }
        }

        private void ClearUniqueCardsSet()
        {
            uniqueCards.Clear();
        }

    }
}
