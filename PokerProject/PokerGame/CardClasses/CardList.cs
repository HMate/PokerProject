using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProject.PokerGame.CardClasses
{
    /*
     * Clas for containing Cards in a Collection.
     * Cannot contain the same card twice.
     * */
    public class CardList : List<PokerCard>, IEquatable<CardList>
    {

        public CardList() : base()
        {
        }

        public CardList(IList<PokerCard> otherList) : base()
        {
            foreach (PokerCard card in otherList)
            {
                Add(new PokerCard(card));
            }
        }

        public CardList(CardList otherList) : base()
        {
            for (int index = 0; index < otherList.Count; index++)
            {
                Add(new PokerCard(otherList.ElementAt(index)));
            }
        }

        public new void Add(PokerCard card)
        {
            if (ContainsThisCard(card))
            {
                throw new InvalidOperationException(String.Format("Card list already contains this card: {0}, {1}",card.Rank, card.Suite));
            }
            base.Add(new PokerCard(card));
        }

        private bool ContainsThisCard(PokerCard card)
        {
            bool containCard = false;
            foreach (PokerCard listedCard in this/*cardList*/)
            {
                if (listedCard.Equals(card))
                {
                    containCard = true;
                    break;
                }
            }
            return containCard;
        }

        public override bool Equals(object obj)
        {
            CardList otherList = obj as CardList;
            if (otherList == null) return false;
            return this.Equals(otherList);
        }

        public bool Equals(CardList otherList)
        {
            if (Count != otherList.Count)
            {
                return false;
            }
            bool equal = true;
            int index = 0;
            while (equal && index < Count)
            {
                //if (! otherList.ElementAt(index).Equals(this.ElementAt(index)))
                //{
                //    equal = false;
                //}
                if (!this.ContainsThisCard(otherList.ElementAt(index)))
                {
                    equal = false;
                }
                if (!otherList.ContainsThisCard(this.ElementAt(index)))
                {
                    equal = false;
                }
                index++;
            }

            return equal;
        }


        public CardRank GetBiggestRank()
        {
            CardRank biggestRank = CardRank.Two;

            foreach (PokerCard card in this)
            {
                if (biggestRank < card.Rank)
                {
                    biggestRank = card.Rank;
                }
            }

            return biggestRank;
        }

        public CardRank GetSmallestRank()
        {
            CardRank smallestRank = CardRank.Ace;

            foreach (PokerCard card in this)
            {
                if (smallestRank > card.Rank)
                {
                    smallestRank = card.Rank;
                }
            }

            return smallestRank;
        }

    }
}
