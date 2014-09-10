using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerProject.PokerGame.CardClasses;
using PokerProject.PokerGame;

namespace PokerUtilityTools
{
    class StatisticalStartCalculator
    {
        public void CalculateStartingValues()
        {
            Dictionary<StarterHand, double> startingValues = new Dictionary<StarterHand, double>();
            CardList allCards = GetHalfDeckWithoutGivenCards(new CardList());
            HashSet<StarterHand> allStarters = new HashSet<StarterHand>();

            CombinatoricsUtilities.GetCombinations<PokerCard>(allCards, startingCards =>
            {
                CardList startingHand = new CardList(startingCards);
                startingHand.Sort();
                allStarters.Add(new StarterHand(startingHand));
            }
            , 2, false);

            int index = 1;
            foreach (StarterHand hand in allStarters)
            {
                double value = GetHighWeightedSquareWithRankAvaragePlayability(hand);
                Console.WriteLine(index++ + "\t" + hand + "\t" + value);
                startingValues.Add(hand, value);
            }

            using (System.IO.FileStream file = new System.IO.FileStream(@"AIfiles/statisticalStartingHands.txt", System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(file, startingValues);
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"AIfiles/statisticalStartingHandsSzöveges.txt"))
            {
                index = 1;
                foreach (var item in startingValues)
                {
                    file.WriteLine(index++ + "\t" + item.Key.ToString() + "\t" + item.Value);
                }
            }
        }

        /// <summary>
        /// Returns the playability value of a given starter hand used for half deck
        /// </summary>
        private double GetApproximatedPlayability(StarterHand myHand)
        {
            //total: 24 choose 3
            double totalCombinations = 2024;
            double playablityCount = 0;
            CardList cardDeck = GetHalfDeckWithoutGivenCards(myHand.GetExampleCards());

            CombinatoricsUtilities.GetCombinations<PokerCard>(cardDeck, imaginaryCards =>
            {
                CardList imaginary5Card = new CardList();
                imaginary5Card.AddRange(imaginaryCards);
                imaginary5Card.AddRange(myHand.GetExampleCards());

                PokerHand imaginaryHand = new PokerHand(imaginary5Card);

                if (imaginaryHand.Category > PokerHand.HandCategory.HighCard)
                {
                    playablityCount += 1;
                }

            }, 3, false);

            return playablityCount * 10 / totalCombinations;
        }

        private double GetApproximatedPlayabilityAgainstHandCategory(StarterHand myHand, PokerProject.PokerGame.PokerHand.HandCategory category)
        {
            //total: 50 choose 3
            double totalCombinations = 19600;
            double playablityCount = 0;
            CardList cardDeck = GetFullDeckWithoutGivenCards(myHand.GetExampleCards());

            CombinatoricsUtilities.GetCombinations<PokerCard>(cardDeck, imaginaryCards =>
            {
                CardList imaginary5Card = new CardList();
                imaginary5Card.AddRange(imaginaryCards);
                imaginary5Card.AddRange(myHand.GetExampleCards());

                PokerHand imaginaryHand = new PokerHand(imaginary5Card);

                if (imaginaryHand.Category > category)
                {
                    playablityCount += 1;
                }

            }, 3, false);

            return playablityCount * 10 / totalCombinations;
        }

        private double GetAvaragePlayability(StarterHand myHand)
        {
            //total: 50 choose 3
            double totalCombinations = 19600;
            double totalCategories = 10;
            Dictionary<PokerProject.PokerGame.PokerHand.HandCategory, double> playability = new Dictionary<PokerHand.HandCategory, double>();
            CardList cardDeck = GetFullDeckWithoutGivenCards(myHand.GetExampleCards());

            CombinatoricsUtilities.GetCombinations<PokerCard>(cardDeck, imaginaryCards =>
            {
                CardList imaginary5Card = new CardList();
                imaginary5Card.AddRange(imaginaryCards);
                imaginary5Card.AddRange(myHand.GetExampleCards());

                PokerHand imaginaryHand = new PokerHand(imaginary5Card);

                foreach (PokerHand.HandCategory category in Enum.GetValues(typeof(PokerHand.HandCategory)))
                {
                    if (imaginaryHand.Category > category && category != PokerHand.HandCategory.RoyalFlush)
                    {
                        playability[category] = playability.ContainsKey(category) ? playability[category] + 1 : 1;
                    }
                }

            }, 3, false);
            
            double sum = playability.Sum(c => c.Value);
            double avarage = sum / totalCategories;

            return avarage * 10 / totalCombinations;
        }

        private double GetHighWeightedAvaragePlayability(StarterHand myHand)
        {
            //total: 50 choose 3
            double totalCombinations = 19600;
            double totalCategories = 10;
            Dictionary<PokerProject.PokerGame.PokerHand.HandCategory, double> playability = new Dictionary<PokerHand.HandCategory, double>();
            CardList cardDeck = GetFullDeckWithoutGivenCards(myHand.GetExampleCards());

            CombinatoricsUtilities.GetCombinations<PokerCard>(cardDeck, imaginaryCards =>
            {
                CardList imaginary5Card = new CardList();
                imaginary5Card.AddRange(imaginaryCards);
                imaginary5Card.AddRange(myHand.GetExampleCards());

                PokerHand imaginaryHand = new PokerHand(imaginary5Card);

                PokerHand.HandCategory category = imaginaryHand.Category;
                playability[category] = playability.ContainsKey(category) ? playability[category] + (double)category : (double)category;

            }, 3, false);

            double sum = playability.Sum(c => c.Value);
            double avarage = sum / totalCategories;

            return avarage * 10 / totalCombinations;
        }

        private double GetHighWeightedSquareWithRankAvaragePlayability(StarterHand myHand)
        {
            //total: 50 choose 3
            double totalCombinations = 19600;
            double totalCategories = 10;
            Dictionary<PokerProject.PokerGame.PokerHand.HandCategory, double> playability = new Dictionary<PokerHand.HandCategory, double>();
            CardList cardDeck = GetFullDeckWithoutGivenCards(myHand.GetExampleCards());

            CombinatoricsUtilities.GetCombinations<PokerCard>(cardDeck, imaginaryCards =>
            {
                CardList imaginary5Card = new CardList();
                imaginary5Card.AddRange(imaginaryCards);
                imaginary5Card.AddRange(myHand.GetExampleCards());

                PokerHand imaginaryHand = new PokerHand(imaginary5Card);

                PokerHand.HandCategory category = imaginaryHand.Category;
                double categoryValue = (double)category * (double)category + (double)imaginaryHand.Rank;
                playability[category] = playability.ContainsKey(category) ? playability[category] + categoryValue : categoryValue;

            }, 3, false);

            double sum = playability.Sum(c => c.Value);
            double avarage = sum / totalCategories;

            return avarage * 10 / totalCombinations;
        }

        private CardList GetHalfDeckWithoutGivenCards(CardList myCards)
        {
            CardList cardDeck = GetFullDeckWithoutGivenCards(myCards);

            CardList copy = new CardList(cardDeck);
            foreach (PokerCard card in copy)
            {
                if (card.Suite != CardSuite.Diamonds && card.Suite != CardSuite.Clubs)
                {
                    cardDeck.Remove(card);
                }
            }

            return cardDeck;
        }

        private CardList GetFullDeckWithoutGivenCards(CardList myCards)
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
