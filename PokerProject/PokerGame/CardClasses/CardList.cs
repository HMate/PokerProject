using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProject.PokerGame.CardClasses
{
    public class CardList
    {
        private List<PokerCard> cardList;

        public CardList()
        {
            cardList = new List<PokerCard>(52);
        }

        public CardList(CardList otherList)
        {
            cardList = new List<PokerCard>(52);
            for (int index = 0; index < otherList.Count; index++)
            {
                cardList.Add(new PokerCard(otherList.ElementAt(index)));
            }
        }

        public int Count
        {
            get
            {
                return cardList.Count;
            }
        }

        public void Add(PokerCard card)
        {
            if (ContainsThisCard(card))
            {
                throw new InvalidOperationException();
            }
            cardList.Add(new PokerCard(card));
        }

        private bool ContainsThisCard(PokerCard card)
        {
            bool containCard = false;
            foreach (PokerCard listedCard in cardList)
            {
                if (listedCard.Equals(card))
                {
                    containCard = true;
                }
            }
            return containCard;
        }

        public PokerCard ElementAt(int index)
        {
            return cardList.ElementAt(index);
        }

        public void RemoveAt(int index)
        {
            cardList.RemoveAt(index);
        }

        public void Clear()
        {
            cardList.Clear();
        }
    }
}
