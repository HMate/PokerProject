using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerProject.PokerGame.CardClasses;
using System.Linq;

namespace PokerUnitTest
{
    [TestClass]
    public class CardListTests
    {
        CardList list;

        [TestInitialize]
        public void SetUp()
        {
            list = new CardList();
        }

        /*
         * A cardList can't contain the same card twice or more.
         * */
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ListContainsUniqueCardsTest()
        {
            list.Add(new PokerCard());
            list.Add(new PokerCard());
        }

        /*
         * A cardList contains the cards in the same order as they are given to it.
         * */
        [TestMethod]
        public void ListContainsCardsTest()
        {
            PokerCard[] referenceCards = new PokerCard[52];
            CreateReferenceCardsAndAddThemToList(referenceCards);

            for (int index = 0; index < 52; index++)
            {
                Assert.AreEqual(referenceCards[index], list.ElementAt(index), "Different cards! reference card:{0}, {1} /nlist card: {2}, {3}"
                    , referenceCards[index].Rank, referenceCards[index].Suite, list.ElementAt(index).Rank, list.ElementAt(index).Suite);
            }
        }

        /*
         * Only the copy of the cards are given to the list, not the card itself.
         * */
        [TestMethod]
        public void ListDontContainReference()
        {
            PokerCard[] referenceCards = new PokerCard[52];
            CreateReferenceCardsAndAddThemToList(referenceCards);

            for (int index = 0; index < 52; index++)
            {
                Assert.AreEqual(referenceCards[index], list.ElementAt(index), "Different cards! reference card:{0}, {1} /nlist card: {2}, {3}"
                    , referenceCards[index].Rank, referenceCards[index].Suite, list.ElementAt(index).Rank, list.ElementAt(index).Suite);

                Assert.AreNotSame(referenceCards[index], list.ElementAt(index), "Cards have the same reference!");
            }
        }

        /*
         * Copy constructor test.
         * Only the copy of the cards are given to the copied list.
         * */
        [TestMethod]
        public void ListCreateFromAnotherList()
        {
            CreateReferenceCardsAndAddThemToList(new PokerCard[52]);
            CardList secondList = new CardList(list);

            Assert.AreNotSame(list, secondList);
            for (int index = 0; index < 52; index++)
            {
                Assert.AreEqual(secondList.ElementAt(index), list.ElementAt(index), "Different cards! reference card:{0}, {1} /nlist card: {2}, {3}"
                    , secondList.ElementAt(index).Rank, secondList.ElementAt(index).Suite, list.ElementAt(index).Rank, list.ElementAt(index).Suite);

                Assert.AreNotSame(secondList.ElementAt(index), list.ElementAt(index), "Cards have the same reference!");
            }
        }

        /*
         * The two lists should be equal if they contain the same cards.
         * */
        [TestMethod]
        public void ListEqualTest()
        {
            CreateReferenceCardsAndAddThemToList(new PokerCard[52]);
            CardList secondList = new CardList(list);

            Assert.AreEqual(list, secondList);
        }

        /*
         * Helper method for tests.
         * Fills up a card array with cards.
         * */
        private void CreateReferenceCardsAndAddThemToList(PokerCard[] referenceCards)
        {
            int index = 0;
            foreach (CardSuite suiteIndex in (CardSuite[])Enum.GetValues(typeof(CardSuite)))
            {
                foreach (CardRank rankIndex in (CardRank[])Enum.GetValues(typeof(CardRank)))
                {
                    referenceCards[index] = new PokerCard(rankIndex, suiteIndex);
                    list.Add(referenceCards[index]);
                    index++;
                }
            }
        }

        [TestMethod]
        public void ListSortTest()
        {
            CardList list = new CardList();
            PokerCard card1 = new PokerCard(CardRank.Three, CardSuite.Diamonds);
            PokerCard card2 = new PokerCard(CardRank.Six, CardSuite.Diamonds);
            PokerCard card3 = new PokerCard(CardRank.Seven, CardSuite.Spades);
            PokerCard card4 = new PokerCard(CardRank.Ten, CardSuite.Hearts);
            PokerCard card5 = new PokerCard(CardRank.Queen, CardSuite.Diamonds);
            list.Add(card3);
            list.Add(card2);
            list.Add(card4);
            list.Add(card5);
            list.Add(card1);

            list.Sort(PokerCard.GetCardComparer());

            Assert.AreEqual(card1, list[0], "Cards don't match at index 0: {0}, {1}", list[0].Rank, list[0].Suite);
            Assert.AreEqual(card2, list[1], "Cards don't match at index 1: {0}, {1}", list[1].Rank, list[1].Suite);
            Assert.AreEqual(card3, list[2], "Cards don't match at index 2: {0}, {1}", list[2].Rank, list[2].Suite);
            Assert.AreEqual(card4, list[3], "Cards don't match at index 3: {0}, {1}", list[3].Rank, list[3].Suite);
            Assert.AreEqual(card5, list[4], "Cards don't match at index 4: {0}, {1}", list[4].Rank, list[4].Suite);
        }
    }
}
