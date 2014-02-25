using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerProject.PokerGame.CardClasses;

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

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ListContainsUniqueCardsTest()
        {
            list.Add(new PokerCard());
            list.Add(new PokerCard());
        }

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
