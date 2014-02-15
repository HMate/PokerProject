using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerProject.PokerGame
{
    public class CardDeck
    {
        private HashSet<PokerCard> cards;

        public CardDeck()
        {
            cards = new HashSet<PokerCard>();
            foreach (int suite in Enum.GetValues(typeof(CardSuite)))
            {
                foreach (int rank in Enum.GetValues(typeof(CardRank)))
                {
                    PokerCard currentCard = new PokerCard(rank, suite);
                    cards.Add(currentCard);
                }
            }
        }

        public PokerCard DealCard()
        {
            return new PokerCard();
        }
    }
}
