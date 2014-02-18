using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using PokerProject.PokerGame;

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
        public void RecreateDeckTest()
        {
            Deal52Cards();
            ClearUniqueCardsSet();
            deck.createNewPokerDeck();
            Deal52Cards();
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

        //Statistical test that fails with 10^-7 chance.
        //[TestMethod]
        public void CardUniquenessTest()
        {
            for (int tries = 0; tries < 1000; tries++)
            {
                int testCycles = 52;
                int failedCycles = 0;
                for (int index = 0; index < testCycles; index++)
                {
                    PokerCard firstCard = deck.DealOneCard();
                    deck.createNewPokerDeck();
                    PokerCard secondCard = deck.DealOneCard();
                    if (firstCard.Equals(secondCard))
                    {
                        failedCycles++;
                    }
                }
                if (failedCycles > 10)
                {
                    Assert.Fail("Get the same result too much times ({0} times at {1}. try)", failedCycles, tries);
                }
            }
            Assert.IsTrue(true);
        }

    }
}
