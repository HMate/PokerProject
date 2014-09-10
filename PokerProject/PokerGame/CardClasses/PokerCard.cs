using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProject.PokerGame.CardClasses
{
    public struct PokerCard : IEquatable<PokerCard>, IComparable<PokerCard>
    {
        private CardSuite suite;
        private CardRank rank;

        public PokerCard(PokerCard card) : this(card.Rank, card.Suite)
        {

        }

        public PokerCard(CardRank cardRank, CardSuite cardSuite)
        {
            rank = cardRank;
            suite = cardSuite;
        }

        public CardSuite Suite { get { return suite; } private set { suite = value; } }

        public CardRank Rank { get { return rank; } private set { rank = value; } }
       
        public override int GetHashCode()
        {
            int rankHash = Rank.GetHashCode();
            return 31 * rankHash + Suite.GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (other is PokerCard)
            {
                return this.Equals((PokerCard)other);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(PokerCard otherCard)
        {
            if ( !this.HasSameRank(otherCard) )
                return false;
            if ( !this.HasSameSuite(otherCard) )
                return false;
            return true;
        }

        public int CompareTo(PokerCard otherCard)
        {
            return (this.HasSameRank(otherCard) ? 0 : ( (this.Rank > otherCard.Rank) ? 1 : -1) );
        }

        public bool HasSameRank(PokerCard otherCard)
        {
            if (Rank.Equals(otherCard.Rank))
            {
                return true;
            }
            return false;
        }

        public bool HasSameSuite(PokerCard otherCard)
        {
            if (Suite.Equals(otherCard.Suite))
            {
                return true;
            }
            return false;
        }

        public static IComparer<PokerCard> GetCardComparer()
        {
            return (IComparer<PokerCard>) new PokerCardComparer();
        }
    }

    public class PokerCardComparer : IComparer<PokerCard>
    {
        int IComparer<PokerCard>.Compare(PokerCard first, PokerCard second)
        {
            return first.CompareTo(second);
        }
    }
}
