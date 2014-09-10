using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerProject.PokerGame.CardClasses;
using PokerProject.PokerGame;
using System.Collections.Generic;

namespace PokerUnitTest
{
    [TestClass]
    public class HandEvaluatorTests
    {

        [TestMethod]
        public void HandEvaluateHighCardTest()
        {
            HandEvaluator evaluator = new HandEvaluator();

            IList<PokerCard> cards = MakeCardList("6C,KH,7S,TD,9S,3C,2H");

            IList<PokerCard> expectedBestCards = MakeCardList("9S,7S,KH,6C,TD");

            evaluator.DetermineBestHand(cards);
            IList<PokerCard> bestCards = evaluator.GetBestCards();
            PokerHand bestHand = evaluator.GetBestHand();

            Assert.AreEqual(expectedBestCards, bestCards);
            Assert.AreEqual(PokerHand.HandCategory.HighCard, bestHand.Category);
        }

        [TestMethod]
        public void HandEvaluatePairTest()
        {
            HandEvaluator evaluator = new HandEvaluator();

            CardList cards = MakeCardList("2C,2H,4S,6D,7S,8S,KC");

            CardList expectedBestCards = MakeCardList("2C,2H,7S,8S,KC");
            
            evaluator.DetermineBestHand(cards);
            CardList bestCards = evaluator.GetBestCards().ToCardList();
            PokerHand bestHand = evaluator.GetBestHand();

            Assert.AreEqual(expectedBestCards, bestCards);
            Assert.AreEqual(PokerHand.HandCategory.Pair, bestHand.Category);
        }

        [TestMethod]
        public void HandEvaluateFulllHouseTest()
        {
            HandEvaluator evaluator = new HandEvaluator();

            CardList cards = MakeCardList("AC,AH,KS,KD,QS,QC,QH");

            CardList expectedBestCards = MakeCardList("AC,AH,QS,QH,QC");

            evaluator.DetermineBestHand(cards);
            CardList bestCards = evaluator.GetBestCards().ToCardList();
            PokerHand bestHand = evaluator.GetBestHand();

            Assert.AreEqual(expectedBestCards, bestCards);
            Assert.AreEqual(PokerHand.HandCategory.FullHouse, bestHand.Category);
        }

        //Helper method for adding cards to tests by string
        private CardList MakeCardList(string cards)
        {
            CardList list = new CardList();
            string[] cardStrings = cards.Split(',');
            foreach (string cardString in cardStrings)
            {
                CardRank rank = CardRank.Two;
                CardSuite suite = CardSuite.Clubs;
                switch (cardString[0])
                {
                    case 'A': rank = CardRank.Ace;
                        break;
                    case 'K': rank = CardRank.King;
                        break;
                    case 'Q': rank = CardRank.Queen;
                        break;
                    case 'J': rank = CardRank.Jack;
                        break;
                    case 'T': rank = CardRank.Ten;
                        break;
                    case '9': rank = CardRank.Nine;
                        break;
                    case '8': rank = CardRank.Eight;
                        break;
                    case '7': rank = CardRank.Seven;
                        break;
                    case '6': rank = CardRank.Six;
                        break;
                    case '5': rank = CardRank.Five;
                        break;
                    case '4': rank = CardRank.Four;
                        break;
                    case '3': rank = CardRank.Three;
                        break;
                    case '2': rank = CardRank.Two;
                        break;
                }
                switch (cardString[1])
                {
                    case 'H': suite = CardSuite.Hearts;
                        break;
                    case 'S': suite = CardSuite.Spades;
                        break;
                    case 'D': suite = CardSuite.Diamonds;
                        break;
                    case 'C': suite = CardSuite.Clubs;
                        break;
                }
                list.Add(new PokerCard(rank, suite));
            }
            return list;
        }
    }
}
