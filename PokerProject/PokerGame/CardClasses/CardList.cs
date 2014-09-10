using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProject.PokerGame.CardClasses
{
    /// <summary>
    /// Class for containing PokerCards in a Collection.
    /// Cannot contain the same card twice.
    /// </summary>
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

        public void AddRange(CardList otherCards)
        {
            foreach (PokerCard card in otherCards)
            {
                this.Add(card);
            }
        }

        public bool ContainsThisCard(PokerCard card)
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

        public new void Sort()
        {
            this.Sort(PokerCard.GetCardComparer());
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

        public override int GetHashCode()
        {
            int rankHash = 0;
            foreach (var item in this)
            {
                rankHash = rankHash * 31 + item.GetHashCode();
            }

            return rankHash;
        }


    }

    public static class CardListHelper
    {
        public static CardList ToCardList(this IList<PokerCard> cards)
        {
            var retList = new CardList();

            retList.AddRange(cards);

            return retList;
        }
    }
}
