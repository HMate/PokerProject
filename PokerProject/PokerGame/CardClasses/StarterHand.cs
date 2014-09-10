using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProject.PokerGame.CardClasses
{
    [Serializable]
    public class StarterHand : IEquatable<StarterHand>
    {
        private CardRank rank1;
        private CardRank rank2;
        private bool offsuite;

        public StarterHand(CardList cards)
        {
            if (cards.Count != 2)
            {
                throw new ArgumentException("I can only create a starter hand from 2 cards", "cards");
            }

            if (cards[0].Rank > cards[1].Rank)
            {
                rank1 = cards[0].Rank;
                rank2 = cards[1].Rank;
            }
            else
            {
                rank1 = cards[1].Rank;
                rank2 = cards[0].Rank;
            }

            offsuite = (cards[0].Suite == cards[1].Suite) ? false : true;
        }

        public CardList GetExampleCards()
        {
            CardList examples = new CardList();
            examples.Add(new PokerCard(rank1, CardSuite.Clubs));
            if (offsuite)
            {
                examples.Add(new PokerCard(rank2, CardSuite.Diamonds));
            }
            else
            {
                examples.Add(new PokerCard(rank2, CardSuite.Clubs));
            }
            return examples;
        }

        public override int GetHashCode()
        {
            return 961 * rank1.GetHashCode() +  31 * rank2.GetHashCode() + offsuite.GetHashCode();
        }

        public override bool Equals(object other)
        {
            StarterHand otherHand = other as StarterHand;
            if (otherHand == null) return false;
            return this.Equals(otherHand);
        }

        public bool Equals(StarterHand otherHand)
        {
            if (otherHand == null) return false;
            if (rank1 != otherHand.rank1 || rank2 != otherHand.rank2 || offsuite != otherHand.offsuite)
                return false;
            return true;
        }

        override public string ToString()
        {
            return String.Format("{0}\t{1}\t{2}", rank1.ToString(), rank2.ToString(), offsuite ? "o" : "s");
        }
    }
}
