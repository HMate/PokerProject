﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerProject.PokerGame.AI_Utilities;
using PokerProject.PokerGame.CardClasses;
using System.Collections.Generic;
using PokerProject.PokerGame;

namespace PokerUnitTest
{
    [TestClass]
    public class HandChancesTest
    {
        #region Pair
        [TestMethod]
        public void ChanceOfPairInCommunityCardsTest()
        {

            CardList holeCards = MakeCardList("6C,KH");
            CardList communityCards = MakeCardList("AS,AH,KH,6C,TD");

            double chance = HandChances.Pair(holeCards, communityCards);

            Assert.AreEqual(1, chance);
        }

        [TestMethod]
        public void ChanceOfPairTest()
        {

            CardList holeCards = MakeCardList("6C,KH");
            CardList communityCards = MakeCardList("AS,DH,5H,7C,TD");

            double chance = HandChances.Pair(holeCards, communityCards);
            double realChance = RealPairChance(holeCards, communityCards);

            //(15C1)*(30C1)/(45C2) = 0,454545
            Assert.AreEqual(realChance, chance, 0.000001); 
        }

        [TestMethod]
        public void ChanceOfPairWithOnePairTest()
        {

            CardList holeCards = MakeCardList("DC,KH");
            CardList communityCards = MakeCardList("AS,DH,5H,7C,TD");

            double chance = HandChances.Pair(holeCards, communityCards);
            double realChance = RealPairChance(holeCards, communityCards);

            //(14C1)*(31C1)/(45C2) = 0,43838383
            Assert.AreEqual(realChance, chance, 0.000001);
        }

        [TestMethod]
        public void ChanceOfPairWithTwoPairTest()
        {

            CardList holeCards = MakeCardList("AC,KH");
            CardList communityCards = MakeCardList("AS,2H,KD,7C,TD");

            double chance = HandChances.Pair(holeCards, communityCards);
            double realChance = RealPairChance(holeCards, communityCards);

            //(13C1)*(32C1)/(45C2) = 0,4202020
            Assert.AreEqual(realChance, chance, 0.000001);
        }

        [TestMethod]
        public void ChanceOfPairWithPairInHandTest()
        {

            CardList holeCards = MakeCardList("AC,AH");
            CardList communityCards = MakeCardList("JS,2H,KD,7C,TD");

            double chance = HandChances.Pair(holeCards, communityCards);
            double realChance = RealPairChance(holeCards, communityCards);

            //(13C1)*(32C1)/(45C2) = 0,4202020
            Assert.AreEqual(realChance, chance, 0.000001);
        }

        [TestMethod]
        public void ChanceOfPairWith3KnownCardsTest()
        {

            CardList holeCards = MakeCardList("AC,AH");
            CardList communityCards = MakeCardList("AS,2H,KD,7C,TD");

            double chance = HandChances.Pair(holeCards, communityCards);
            double realChance = RealPairChance(holeCards, communityCards);

            //(13C1)*(32C1)/(45C2) = 0,4202020
            Assert.AreEqual(realChance, chance, 0.000001);
        }

        [TestMethod]
        public void ChanceOfPairWith3KnownCardsSpeedTest()
        {

            CardList holeCards = MakeCardList("AC,AH");
            CardList communityCards = MakeCardList("AS,2H,KD,7C,TD");

            double chance = HandChances.Pair(holeCards, communityCards);

            //(13C1)*(32C1) + 8*(4C2) /(45C2) = 0,4686868
            Assert.AreEqual(0.4686868D, chance, 0.000001);
        }
        #endregion

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

        double RealPairChance(CardList holeCards, CardList communityCards)
        {
            HandEvaluator evaluator = new HandEvaluator();
            CardList knownCards = new CardList(communityCards);
            knownCards.AddRange(holeCards);
            int pairs = 0;
            int total = 0;
            CardList possibleOpponentCards = GetFullDeckWithoutGivenCards(knownCards);
            CombinatoricsUtilities.GetCombinations<PokerCard>(possibleOpponentCards, opponentCards =>
            {
                CardList opponent7Card = new CardList(communityCards);
                opponent7Card.AddRange(opponentCards);
                evaluator.DetermineBestHand(opponent7Card);

                PokerHand opponentHand = evaluator.GetBestHand();

                if (opponentHand.Category.Equals(PokerHand.HandCategory.Pair))
                {
                    pairs += 1;
                }
                total++;
            }, 2, false);

            return (double)pairs / total;
        }

        protected CardList GetFullDeckWithoutGivenCards(CardList myCards)
        {
            CardList cardDeck = new CardList();
            foreach (CardSuite suiteIndex in (CardSuite[])Enum.GetValues(typeof(CardSuite)))
            {
                foreach (CardRank rankIndex in (CardRank[])Enum.GetValues(typeof(CardRank)))
                {
                    cardDeck.Add(new PokerCard(rankIndex, suiteIndex));
                }
            }

            foreach (PokerCard card in myCards)
            {
                cardDeck.Remove(card);
            }

            return cardDeck;
        }
    }
}
