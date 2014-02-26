using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProject.PokerGame.CardClasses
{
    public class CardList : List<PokerCard>
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

    }
}
