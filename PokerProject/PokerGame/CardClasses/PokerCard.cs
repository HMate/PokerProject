using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProject.PokerGame.CardClasses
{
    public class PokerCard : IEquatable<PokerCard>, IComparable<PokerCard>
    {
        private CardRank rank;
        private CardSuite suite;

        public PokerCard(PokerCard card) : this(card.Rank, card.Suite)
        {

        }

        public PokerCard(CardRank cardRank, CardSuite cardSuite)
        {
            rank = cardRank;
            suite = cardSuite;
        }

        public PokerCard()
            : this(CardRank.Ace, CardSuite.Spades)
        {

        }

        public CardSuite Suite
        {
            get
            {
                return suite;
            }
         }

        public CardRank Rank
        {
            get
            {
                return rank;
            }

        }
       
        public override int GetHashCode()
        {
            int rankHash = rank.GetHashCode();
            return 31*rankHash + suite.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            PokerCard otherCard = obj as PokerCard;
            if (otherCard == null) return false;
            return this.Equals(otherCard);
        }

        public bool Equals(PokerCard otherCard)
        {
            if (otherCard == null) return false;
            if ( !this.HasSameRank(otherCard) )
                return false;
            if ( !this.HasSameSuite(otherCard) )
                return false;
            return true;
        }

        public int CompareTo(PokerCard otherCard)
        {
            if (otherCard == null)
            {
                return 1;
            }

            return (this.HasSameRank(otherCard) ? 0 : ( (this.Rank > otherCard.Rank) ? 1 : -1) );
        }

        public bool HasSameRank(PokerCard otherCard)
        {
            if (otherCard == null)
            {
                return false;
            }
            if (rank.Equals(otherCard.rank))
            {
                return true;
            }
            return false;
        }

        public bool HasSameSuite(PokerCard otherCard)
        {
            if (otherCard == null)
            {
                return false;
            }
            if (suite.Equals(otherCard.suite))
            {
                return true;
            }
            return false;
        }

        public static IComparer<PokerCard> GetCardComparer()
        {
            return (IComparer<PokerCard>) new PokerCardComparer();
        }

        private class PokerCardComparer : IComparer<PokerCard>
        {
            int IComparer<PokerCard>.Compare(PokerCard first, PokerCard second)
            {
                if (first == null)
                {
                    if (second == null)
                    {
                        return 0;
                    }

                    return -1;
                }

                return first.CompareTo(second);
            }
        }

    }
}
