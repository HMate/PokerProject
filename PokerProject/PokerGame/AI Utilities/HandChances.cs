using PokerProject.PokerGame.CardClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProject.PokerGame.AI_Utilities
{
    public static class HandChances
    {
        static HandEvaluator evaluator = new HandEvaluator();

        /// <summary>
        /// Gives the chances that somebody has a pair.
        /// Needs to have 5 communityCards.
        /// </summary>
        /// <param name="holeCards"></param>
        /// <param name="communityCards"></param>
        /// <returns></returns>
        public static double Pair(CardList holeCards, CardList communityCards)
        {
            if (PokerHand.IsPair(communityCards)) return 1;

            CardList knownCards = new CardList(communityCards);
            knownCards.AddRange(holeCards);
            int deckSize = 52;
            int possibleCards = deckSize - knownCards.Count;
            int unknownCards = 7 - communityCards.Count;

            int possiblePairCards = communityCards.Count * 3;
            int knownPairCards = 0;

            if (PokerHand.IsPair(knownCards))
            {
                foreach (var handCard in holeCards)
                {
                    foreach (var card in communityCards)
                    {
                        if (handCard.Rank == card.Rank)
                        {
                            knownPairCards++;
                        }
                    }
                }
            }


            //TODO: Not counting when opponent has a pair in his hand

            possiblePairCards = possiblePairCards - knownPairCards;
            return (possiblePairCards * Combinations(possibleCards - possiblePairCards, unknownCards - 1)) / Combinations(possibleCards, unknownCards);
        }
                
        public static double Combinations(int n, int k)
        {
            return Fact(n) / (Fact(k) * Fact(n - k));
        }

        public static double Fact(int n)
        {
            return Fact(n, 1);
        }

        private static double Fact(int n, double Acc)
        {
            if (n == 1) return Acc;
            return Fact(n - 1, Acc * n);
        }
    }
}
