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
                throw new InvalidOperationException();
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
            bool equal = true;
            int index = 0;
            while (equal && index < Count)
            {
                if (! otherList.ElementAt(index).Equals(this.ElementAt(index)))
                {
                    equal = false;
                }
                index++;
            }

            return equal;
        }

    }
}
