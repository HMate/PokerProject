using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProject.PokerGame
{
    public class PokerCard : IEquatable<PokerCard>
    {
        private CardRank rank;
        private CardSuite suite;

        public PokerCard(CardRank cardRank, CardSuite cardSuite)
        {
            rank = cardRank;
            suite = cardSuite;
        }

        public PokerCard() : this(CardRank.Ace, CardSuite.Spades)
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
            //TODO: better Hashcode
            rank.GetHashCode();
            return ((int)rank << 3) ^ (int)suite;
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


    }
}
