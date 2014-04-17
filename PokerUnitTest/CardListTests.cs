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
    }
}
