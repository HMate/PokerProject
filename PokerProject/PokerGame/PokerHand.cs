using PokerProject.PokerGame.CardClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProject.PokerGame
{
    public class PokerHand : IComparable<PokerHand>, IEquatable<PokerHand>
    {
        private HandCategory category;
        private CardRank rank;
        private CardRank secondRank;
        private CardList kickers;

        public enum HandCategory
        {
            HighCard = 0,
            Pair = 1,
            TwoPair = 2,
            ThreeOfAKind = 3,
            Straight = 4,
            Flush = 5,
            FullHouse = 6,
            Poker = 7,
            StraightFlush = 8,
            RoyalFlush = 9
        }

        public HandCategory Category
        {
            get
            {
                return category;
            }
        }

        public CardRank Rank
        {
            get
            {
                return rank;
            }
        }

        public CardRank SecondRank
        {
            get
            {
                return secondRank;
            }
        }

        public CardList Kickers
        {
            get
            {
                return kickers;
            }
        }

        public PokerHand(CardList cards)
        {
            kickers = new CardList();
            secondRank = 0;
            SetPokerHand(cards);
        }

        private void SetPokerHand(CardList cards)
        {
            if (IsStraightFlush(cards))
            {
                SetStraightFlushFields(cards);
            }
            else if (IsPoker(cards))
            {
                SetPokerFields(cards);
            }
            else if (IsFullHouse(cards))
            {
                SetFullHouseFields(cards);
            }
            else if (IsFlush(cards))
            {
                SetFlushFields(cards);
            }
            else if (IsStraight(cards))
            {
                SetStraightFields(cards);
            }
            else if (IsThreeOfAKind(cards))
            {
                SetThreeOfAKindFields(cards);
            }
            else if (IsTwoPair(cards))
            {
                SetTwoPairFields(cards);
            }
            else if (IsPair(cards))
            {
                SetPairFields(cards);
            }
            else
            {
                SetHighCardFields(cards);
            }
        }

        /// <summary>
        /// Checks wether the given card list matches the hand category: Straightflush
        /// </summary>
        public static bool IsStraightFlush(CardList cards)
        {
            bool isStraightFlush = false;
            if (IsFlush(cards))
            {
                if (IsStraight(cards))
                {
                    isStraightFlush = true;
                }
            }

            return isStraightFlush;
        }

        private void SetStraightFlushFields(CardList cards)
        {
            rank = cards.GetBiggestRank();
            category = HandCategory.StraightFlush;
            if (rank == CardRank.Ace)
            {
                category = HandCategory.RoyalFlush;
            }
            //no kicker needed, handcategory already tells hand strenght.
        }

        /// <summary>
        /// Checks wether the given card list matches the hand category: Poker
        /// </summary>
        public static bool IsPoker(CardList cards)
        {
            bool isPoker = false;

            //Go throught every combination of 3 cards. if these have the same rank, its three of a kind.
            CombinatoricsUtilities.GetCombinations(cards, fours =>
            {
                if ((fours[0].Rank == fours[1].Rank) &&
                    (fours[0].Rank == fours[2].Rank) && (fours[0].Rank == fours[3].Rank))
                {
                    isPoker = true;
                }
            }, 4, false);

            return isPoker;
        }

        private void SetPokerFields(CardList cards)
        {
            category = HandCategory.Poker;
            //Determine PokerHand's rank
            if (cards[0].Rank == cards[1].Rank)
            {
                rank = cards[0].Rank;
            }
            else if (cards[0].Rank == cards[2].Rank)
            {
                rank = cards[0].Rank;
            }
            else
            {
                rank = cards[1].Rank;
            }

            FillKickersByRankFromList(cards);
        }

        /// <summary>
        /// Checks wether the given card list matches the hand category: Full House
        /// </summary>
        public static bool IsFullHouse(CardList cards)
        {
            bool isFullHouse = true;

            //Check for full house: Every card should have at least one pair.
            //If a card dont have a pair, its not full house.
            foreach (PokerCard card in cards)
            {
                bool foundPair = false;
                CardList restOfCards = new CardList(cards);
                restOfCards.Remove(card);

                foreach (PokerCard otherCard in restOfCards)
                {
                    if (card.Rank == otherCard.Rank)
                    {
                        foundPair = true;
                    }
                }

                if (!foundPair)
                {
                    isFullHouse = false;
                }
            }

            return isFullHouse;
        }
        
        private void SetFullHouseFields(CardList cards)
        {
            category = HandCategory.FullHouse;
            //Set rank
            CardRank fhRank = cards[0].Rank;
            int rankCounter = 0;
            CardRank fhSecondRank = 0;
            foreach (PokerCard card in cards)
            {
                if (fhRank == card.Rank)
                {
                    rankCounter++;
                }
                else
                {
                    fhSecondRank = card.Rank;
                }
            }

            if (rankCounter == 3)
            {
                rank = fhRank;
                secondRank = fhSecondRank;
            }
            else
            {
                rank = fhSecondRank;
                secondRank = fhRank;
            }
            //no kickers needed, ranks already tells hand strenght.
        }

        /// <summary>
        /// Checks wether the given card list matches the hand category: Flush
        /// </summary>
        public static bool IsFlush(CardList cards)
        {
            CardSuite suite = cards.ElementAt(0).Suite;

            foreach (PokerCard card in cards)
            {
                if (card.Suite != suite)
                {
                    return false;
                }
            }

            return true;
        }
        
        private void SetFlushFields(CardList cards)
        {
            category = HandCategory.Flush;
            kickers = new CardList(cards);
            rank = cards.GetBiggestRank();
        }

        /// <summary>
        /// Checks wether the given card list matches the hand category: Straight
        /// </summary>
        public static bool IsStraight(CardList cards)
        {
            CardRank smallestRank = cards.GetSmallestRank();

            for (CardRank nextRank = smallestRank;
                nextRank < (CardRank)(smallestRank + 5);
                nextRank = (CardRank)(nextRank + 1))
            {
                bool hasRank = false;

                foreach (PokerCard card in cards)
                {
                    if (card.Rank == nextRank)
                    {
                        hasRank = true;
                    }
                }
                if (hasRank == false)
                {
                    return false;
                }
            }

            return true;
        }
        
        private void SetStraightFields(CardList cards)
        {
            category = HandCategory.Straight;
            rank = cards.GetBiggestRank();
            //no kicker needed, handcategory already tells hand strenght.
        }

        /// <summary>
        /// Checks wether the given card list matches the hand category: Three of a kind
        /// </summary>
        public static bool IsThreeOfAKind(CardList cards)
        {
            bool isThreeOfAKind = false;

            //Go throught every combination of 3 cards. if these have the same rank, its three of a kind.
            CombinatoricsUtilities.GetCombinations(cards, threes =>
            {
                if ((threes[0].Rank == threes[1].Rank) && (threes[1].Rank == threes[2].Rank))
                {
                    isThreeOfAKind = true;
                }
            }, 3, false);

            return isThreeOfAKind;
        }
        
        private void SetThreeOfAKindFields(CardList cards)
        {
            category = HandCategory.ThreeOfAKind;

            //SetRank
            CardRank lastRank = cards[0].Rank;

            for (int i = 1; i < cards.Count; i++)
            {
                if (cards[i].Rank == lastRank)
                {
                    break;
                }
                else
                {
                    lastRank = cards[i].Rank;
                }
            }
            //if there wasn't 2 consecutive cards with the same rank lastRank will be correct
            //if loop ended with break lastRank will be correct
            rank = lastRank;

            FillKickersByRankFromList(cards);
        }

        /// <summary>
        /// Checks wether the given card list matches the hand category: Two pairs
        /// </summary>
        public static bool IsTwoPair(CardList cards)
        {
            bool isPair = false;
            bool isTwoPair = false;

            //Go throught every combination of 2 cards. if these have the same rank, its a pair.
            CombinatoricsUtilities.GetCombinations(cards, pair =>
            {
                if (pair[0].Rank == pair[1].Rank)
                {
                    if (!isPair)
                    {
                        isPair = true;
                    }
                    else
                    {
                        isTwoPair = true;
                    }
                }
            }, 2, false);

            return isTwoPair;
        }
        
        private void SetTwoPairFields(CardList cards)
        {
            category = HandCategory.TwoPair;

            //set rank
            bool foundAPair = false;
            foreach (PokerCard card in cards)
            {
                bool hasSameRank = false;
                CardList restOfCards = new CardList(cards);
                restOfCards.Remove(card);

                foreach (PokerCard otherCard in restOfCards)
                {
                    if (otherCard.Rank == card.Rank)
                    {
                        hasSameRank = true;
                        if (foundAPair == false)
                        {
                            rank = card.Rank;
                            foundAPair = true;
                        }
                        else if (rank != card.Rank)
                        {
                            secondRank = card.Rank;
                        }
                    }
                }

                if (hasSameRank == false)
                {
                    //only one card should be a kicker in TwoPairs hand
                    kickers.Add(card);
                }
            }

            if (rank < secondRank)
            {
                CardRank temp = rank;
                rank = secondRank;
                secondRank = temp;
            }

        }

        /// <summary>
        /// Checks wether the given card list matches the hand category: One pair
        /// </summary>
        public static bool IsPair(CardList cards)
        {
            bool isPair = false;

            //Go throught every combination of 2 cards. if these have the same rank, its a pair.
            CombinatoricsUtilities.GetCombinations(cards, pair =>
            {
                if (pair[0].Rank == pair[1].Rank)
                {
                    isPair = true;
                }
            }, 2, false);

            return isPair;
        }

        private void SetPairFields(CardList cards)
        {
            category = HandCategory.Pair;
            // set rank
            CombinatoricsUtilities.GetCombinations(cards, pair =>
            {
                if (pair[0].Rank == pair[1].Rank)
                {
                    rank = pair[0].Rank;
                }
            }, 2, false);
            //set kickers
            FillKickersByRankFromList(cards);
        }

        private void SetHighCardFields(CardList cards)
        {
            category = HandCategory.HighCard;
            rank = cards.GetBiggestRank();
            kickers = new CardList(cards);
        }

        private void FillKickersByRankFromList(CardList cards)
        {
            foreach (PokerCard card in cards)
            {
                if (card.Rank != this.Rank)
                {
                    kickers.Add(card);
                }
            }
        }

        /*
         * Compare the hands
         * return 1 if this hand is bigger than the other.
         * return 0 if they have the same value
         * return -1 if the other hand is the bigger.
         * */
        public int CompareTo(PokerHand other)
        {
            if ((object)other == null)
            {
                return 1;
            }

            if (this.category > other.category)
            {
                return 1;
            }
            else if (this.category < other.category)
            {
                return -1;
            }

            // at this point the hands have the same category
            if (this.Rank > other.Rank)
            {
                return 1;
            }
            else if (this.Rank < other.Rank)
            {
                return -1;
            }

            if (this.SecondRank > other.SecondRank)
            {
                return 1;
            }
            else if (this.SecondRank < other.SecondRank)
            {
                return -1;
            }

            //at this point hands have the same category and rank, so kickers decide
            if (kickers.Count > 0)
            {
                IComparer<PokerCard> comparer = PokerCard.GetCardComparer();
                kickers.Sort(comparer);
                other.kickers.Sort(comparer);

                for (int index = kickers.Count - 1; index > -1; index--)
                {
                    if (kickers[index].Rank > other.kickers[index].Rank)
                    {
                        return 1;
                    }
                    else if (kickers[index].Rank < other.kickers[index].Rank)
                    {
                        return -1;
                    }
                }
            }

            return 0;
        }

        //Operator overloading
        public static bool operator >(PokerHand firstHand, PokerHand secondHand)
        {
            if ((object)firstHand == null)
            {
                return false;
            }
            if ((object)secondHand == null)
            {
                return true;
            }
            return firstHand.CompareTo(secondHand) > 0;
        }

        public static bool operator <(PokerHand firstHand, PokerHand secondHand)
        {
            if ((object)firstHand == null)
            {
                return true;
            }
            if ((object)secondHand == null)
            {
                return false;
            }
            return firstHand.CompareTo(secondHand) < 0;
        }

        public static bool operator ==(PokerHand firstHand, PokerHand secondHand)
        {
            if ((object)firstHand == null)
            {
                return false;
            }
            if ((object)secondHand == null)
            {
                return false;
            }
            return firstHand.CompareTo(secondHand) == 0;
        }

        public static bool operator !=(PokerHand firstHand, PokerHand secondHand)
        {
            return !(firstHand == secondHand);
        }

        public override bool Equals(object obj)
        {
            PokerHand otherHand = obj as PokerHand;
            if (otherHand == null)
            {
                return false;
            }
            return this == otherHand;
        }

        public bool Equals(PokerHand otherHand)
        {
            return this == otherHand;
        }

        public override int GetHashCode()
        {
            int hash = rank.GetHashCode();
            hash = 31 * hash + secondRank.GetHashCode();
            hash = 31 * hash + category.GetHashCode();
            return hash;
        }


    }
}
