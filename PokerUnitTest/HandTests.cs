using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerProject.PokerGame;
using PokerProject.PokerGame.CardClasses;

namespace PokerUnitTest
{
    [TestClass]
    public class HandTests
    {

        [TestMethod]
        public void HandCategoryCompareTest()
        {
            Assert.IsTrue(PokerHand.HandCategory.HighCard < PokerHand.HandCategory.Pair);
            Assert.IsTrue(PokerHand.HandCategory.Pair < PokerHand.HandCategory.TwoPair);
            Assert.IsTrue(PokerHand.HandCategory.TwoPair < PokerHand.HandCategory.ThreeOfAKind);
            Assert.IsTrue(PokerHand.HandCategory.ThreeOfAKind < PokerHand.HandCategory.Straight);
            Assert.IsTrue(PokerHand.HandCategory.Straight < PokerHand.HandCategory.Flush);
            Assert.IsTrue(PokerHand.HandCategory.Flush < PokerHand.HandCategory.FullHouse);
            Assert.IsTrue(PokerHand.HandCategory.FullHouse < PokerHand.HandCategory.Poker);
            Assert.IsTrue(PokerHand.HandCategory.Poker < PokerHand.HandCategory.StraightFlush);

            Assert.IsTrue(PokerHand.HandCategory.Poker == PokerHand.HandCategory.Poker);
        }

        [TestMethod]
        public void HandFindHighCardTest()
        {
            CardList cards = MakeCardList("3C,4H,6S,JD,AS");

            PokerHand hand = new PokerHand(cards);
            CardList kickers = hand.Kickers;

            Assert.AreEqual(PokerHand.HandCategory.HighCard, hand.Category);
            Assert.AreEqual(CardRank.Ace, hand.Rank, "Wrong hand rank!");
            CollectionAssert.Contains(kickers, new PokerCard(CardRank.Three, CardSuite.Clubs));
            CollectionAssert.Contains(kickers, new PokerCard(CardRank.Four, CardSuite.Hearts));
            CollectionAssert.Contains(kickers, new PokerCard(CardRank.Six, CardSuite.Spades));
            CollectionAssert.Contains(kickers, new PokerCard(CardRank.Jack, CardSuite.Diamonds));
            CollectionAssert.Contains(kickers, new PokerCard(CardRank.Ace, CardSuite.Spades));
        }

        //Check for a pair
        [TestMethod]
        public void HandFindPairTest()
        {
            CardList cards = MakeCardList("3C,4H,3S,5D,6S");

            PokerHand hand = new PokerHand(cards);
            CardList kickers = hand.Kickers;

            Assert.AreEqual(PokerHand.HandCategory.Pair, hand.Category);
            Assert.AreEqual(CardRank.Three, hand.Rank, "Wrong hand rank!");
            CollectionAssert.Contains(kickers, new PokerCard(CardRank.Four, CardSuite.Hearts));
            CollectionAssert.Contains(kickers, new PokerCard(CardRank.Five, CardSuite.Diamonds));
            CollectionAssert.Contains(kickers, new PokerCard(CardRank.Six, CardSuite.Spades));
            CollectionAssert.DoesNotContain(kickers, new PokerCard(CardRank.Three, CardSuite.Clubs), "There's a card from the pair among the kickers.", CardSuite.Clubs);
            CollectionAssert.DoesNotContain(kickers, new PokerCard(CardRank.Three, CardSuite.Spades), "There's a card from the pair among the kickers.", CardSuite.Spades);
        }

        [TestMethod]
        public void HandFindTwoPairTest()
        {
            CardList cards = MakeCardList("3C,7H,3S,7D,AS");

            PokerHand hand = new PokerHand(cards);
            CardList kickers = hand.Kickers;

            Assert.AreEqual(PokerHand.HandCategory.TwoPair, hand.Category);
            Assert.AreEqual(CardRank.Seven, hand.Rank, "Wrong hand rank!");
            CollectionAssert.Contains(kickers, new PokerCard(CardRank.Ace, CardSuite.Spades));
            CollectionAssert.DoesNotContain(kickers, new PokerCard(CardRank.Three, CardSuite.Spades), "There's a card from the smaller pair among the kickers.");
            CollectionAssert.DoesNotContain(kickers, new PokerCard(CardRank.Seven, CardSuite.Diamonds), "There's a card from the bigger pair among the kickers.");
        }

        [TestMethod]
        public void HandFindThreeOfAKindTest()
        {
            CardList cards = MakeCardList("3C,3H,4S,3D,AS");

            PokerHand hand = new PokerHand(cards);
            CardList kickers = hand.Kickers;

            Assert.AreEqual(PokerHand.HandCategory.ThreeOfAKind, hand.Category);
            Assert.AreEqual(CardRank.Three, hand.Rank, "Wrong hand rank!");
            CollectionAssert.Contains(kickers, new PokerCard(CardRank.Four, CardSuite.Spades));
            CollectionAssert.Contains(kickers, new PokerCard(CardRank.Ace, CardSuite.Spades));
        }

        [TestMethod]
        public void HandFindStraightTest()
        {
            CardList cards = MakeCardList("4H,5S,6S,7S,8C");

            PokerHand hand = new PokerHand(cards);
            CardList kickers = hand.Kickers;

            Assert.AreEqual(PokerHand.HandCategory.Straight, hand.Category);
            Assert.AreEqual(CardRank.Eight, hand.Rank, "Wrong hand rank!");
            Assert.IsTrue(kickers.Count == 0);
        }

        [TestMethod]
        public void HandFindFlushTest()
        {
            CardList cards = MakeCardList("4H,5H,7H,9H,QH");

            PokerHand hand = new PokerHand(cards);
            CardList kickers = hand.Kickers;

            Assert.AreEqual(PokerHand.HandCategory.Flush, hand.Category);
            Assert.AreEqual(CardRank.Queen, hand.Rank, "Wrong hand rank!");
            Assert.IsTrue(kickers.Count == 5);
        }

        [TestMethod]
        public void HandFindFullHouseTest()
        {
            CardList cards = MakeCardList("4H,4S,KC,KD,KH");

            PokerHand hand = new PokerHand(cards);
            CardList kickers = hand.Kickers;

            Assert.AreEqual(PokerHand.HandCategory.FullHouse, hand.Category);
            Assert.AreEqual(CardRank.King, hand.Rank, "Wrong hand rank!");
            Assert.IsTrue(kickers.Count == 0);
        }

        [TestMethod]
        public void HandFindPokerTest()
        {
            CardList cards = MakeCardList("4H,KS,KC,KD,KH");

            PokerHand hand = new PokerHand(cards);
            CardList kickers = hand.Kickers;

            Assert.AreEqual(PokerHand.HandCategory.Poker, hand.Category);
            Assert.AreEqual(CardRank.King, hand.Rank, "Wrong hand rank!");
            CollectionAssert.Contains(kickers, new PokerCard(CardRank.Four, CardSuite.Hearts));
        }

        [TestMethod]
        public void HandFindStraightFlushTest()
        {
            CardList cards = MakeCardList("3S,4S,5S,6S,7S");

            PokerHand hand = new PokerHand(cards);
            CardList kickers = hand.Kickers;

            Assert.AreEqual(PokerHand.HandCategory.StraightFlush, hand.Category);
            Assert.AreEqual(CardRank.Seven, hand.Rank);
            Assert.IsTrue(kickers.Count == 0);
        }

        [TestMethod]
        public void HandFindRoyalFlushTest()
        {
            CardList cards = MakeCardList("TD,JD,QD,KD,AD");

            PokerHand hand = new PokerHand(cards);
            CardList kickers = hand.Kickers;

            Assert.AreEqual(PokerHand.HandCategory.RoyalFlush, hand.Category);
            Assert.AreEqual(CardRank.Ace, hand.Rank);
            Assert.IsTrue(kickers.Count == 0);
        }

        [TestMethod]
        public void HandCompareTest()
        {
            CardList winnerCards = MakeCardList("TD,JD,QD,KD,AD");
            CardList loserCards = MakeCardList("3C,3D,3H,TS,TD");

            PokerHand winnerHand = new PokerHand(winnerCards);
            PokerHand loserHand = new PokerHand(loserCards);

            Assert.IsTrue(winnerHand.CompareTo(loserHand) == 1, "Winner hand didn't won!");
            Assert.IsTrue(loserHand.CompareTo(winnerHand) == -1, "Loser hand didn't lost!");
        }

        [TestMethod]
        public void HandCompareSameCategoryHandsTest()
        {
            CardList winnerCards = MakeCardList("3H,2C,2S,QD,QH");
            CardList loserCards = MakeCardList("6D,6C,4S,2H,2D");

            PokerHand winnerHand = new PokerHand(winnerCards);
            PokerHand loserHand = new PokerHand(loserCards);

            Assert.IsTrue(winnerHand.CompareTo(loserHand) == 1, "Winner hand didn't won!");
            Assert.IsTrue(loserHand.CompareTo(winnerHand) == -1, "Loser hand didn't lost!");
        }

        [TestMethod]
        public void HandCompareTwoPairKickersTest()
        {
            CardList winnerCards = MakeCardList("6D,6C,4S,2H,2D");
            CardList loserCards = MakeCardList("6S,6H,3H,2C,2S");

            PokerHand winnerHand = new PokerHand(winnerCards);
            PokerHand loserHand = new PokerHand(loserCards);

            Assert.IsTrue(winnerHand.CompareTo(loserHand) == 1, "Winner hand didn't won!");
            Assert.IsTrue(loserHand.CompareTo(winnerHand) == -1, "Loser hand didn't lost!");
        }

        [TestMethod]
        public void HandComparePairKickersTest()
        {
            CardList winnerCards = MakeCardList("6D,6C,4S,2H,TD");
            CardList loserCards = MakeCardList("6S,6H,5H,7C,8S");

            PokerHand winnerHand = new PokerHand(winnerCards);
            PokerHand loserHand = new PokerHand(loserCards);

            Assert.IsTrue(winnerHand.CompareTo(loserHand) == 1, "Winner hand didn't won!");
            Assert.IsTrue(loserHand.CompareTo(winnerHand) == -1, "Loser hand didn't lost!");
        }

        [TestMethod]
        public void HandCompareTwoPairSecondPairTest()
        {
            CardList winnerCards = MakeCardList("6D,6C,3S,2H,3D");
            CardList loserCards = MakeCardList("6S,6H,AH,2C,2S");

            PokerHand winnerHand = new PokerHand(winnerCards);
            PokerHand loserHand = new PokerHand(loserCards);

            Assert.IsTrue(winnerHand.CompareTo(loserHand) == 1, "Winner hand didn't won!");
            Assert.IsTrue(loserHand.CompareTo(winnerHand) == -1, "Loser hand didn't lost!");
        }

        [TestMethod]
        public void HandCompareTieHandsTest()
        {
            CardList tieCards = MakeCardList("6C,5C,3S,2C,KC");
            CardList tieCards2 = MakeCardList("KS,6H,2H,3C,5S");

            PokerHand tieHand = new PokerHand(tieCards);
            PokerHand tieHand2 = new PokerHand(tieCards2);

            Assert.IsTrue(tieHand.CompareTo(tieHand2) == 0, "Tie hand 1 won during tie!");
            Assert.IsTrue(tieHand2.CompareTo(tieHand) == 0, "Tie hand 2 won during tie!");
        }


        //Helper method for adding cards to tests
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
