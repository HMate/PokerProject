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
            Deal52CardsOneByOne();
            Assert.IsTrue(true);
        }

        private void Deal52CardsOneByOne()
        {
            for (int cardIndex = 0; cardIndex < CardDeck.defaultDeckSize; cardIndex++)
            {
                PokerCard testCard = deck.DealOneCard();
                bool success = uniqueCards.Add(testCard);

                if (!success)
                {
                    Assert.Fail("Couldn't add card ({0} of {1}) to the set at index {2}", testCard.Rank, testCard.Suite, cardIndex);
                }
            }
        }

        [TestMethod]
        public void DeckDealFullDeckTest()
        {
            CardList list = deck.DealCards(52);
            uniqueCards.UnionWith(list);
            Assert.AreEqual(52, uniqueCards.Count);
        }

        [TestMethod]
        public void DealMoreCardsThan52CardsTest()
        {
            try
            {
                deck.DealCards(53);
            }
            catch(CardDeckEmptyException)
            {
                return;
            }
            Assert.Fail();
        }

        [TestMethod]
        public void DeckSizeRemainingTest()
        {
            deck.DealCards(52);
            Assert.AreEqual(0, deck.GetDeckSize());
        }

        [TestMethod]
        public void DeckRecreateTest()
        {
            deck.DealCards(52);
            Assert.AreEqual(0, deck.GetDeckSize());
            deck.createNewPokerDeck();
            Assert.AreEqual(52, deck.GetDeckSize());
        }
    }
}
