using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerProject.PokerGame.CardClasses;
using PokerProject.PokerGame;

namespace PokerUnitTest
{
    [TestClass]
    public class HandEvaluatorTests
    {
        
        [TestMethod]
        public void HandEvaluateTest()
        {
            HandEvaluator evaluator = new HandEvaluator();

            CardList cards = new CardList();
            // 2C, 2H, 4S, 6D, 7S, 8S, KC
            cards.Add(new PokerCard(CardRank.Two, CardSuite.Clubs));
            cards.Add(new PokerCard(CardRank.Two, CardSuite.Hearts));
            cards.Add(new PokerCard(CardRank.Four, CardSuite.Spades));
            cards.Add(new PokerCard(CardRank.Six, CardSuite.Diamonds));
            cards.Add(new PokerCard(CardRank.Seven, CardSuite.Spades));
            cards.Add(new PokerCard(CardRank.Eight, CardSuite.Spades));
            cards.Add(new PokerCard(CardRank.King, CardSuite.Clubs));

            //best hand: 2C, 2H, 7S, 8S, KC
            CardList expectedBestHand = new CardList();
            expectedBestHand.Add(new PokerCard(CardRank.Two, CardSuite.Clubs));
            expectedBestHand.Add(new PokerCard(CardRank.Two, CardSuite.Hearts));
            expectedBestHand.Add(new PokerCard(CardRank.Seven, CardSuite.Spades));
            expectedBestHand.Add(new PokerCard(CardRank.Eight, CardSuite.Spades));
            expectedBestHand.Add(new PokerCard(CardRank.King, CardSuite.Clubs));

            CardList bestHand = evaluator.DetermineBestHand(cards);

            Assert.AreEqual(expectedBestHand, bestHand);

        }
    }
}
