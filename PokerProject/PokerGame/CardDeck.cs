using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerProject.PokerGame
{
    public class CardDeck
    {
        private List<PokerCard> deck;
        private Random randomGenerator = new Random();
        private const int defaultDeckSize = 52;

        public CardDeck()
        {
            deck = new List<PokerCard>(52);
            createNewPokerDeck();
        }

        public void createNewPokerDeck()
        {
            foreach (CardSuite suiteIndex in (CardSuite[])Enum.GetValues(typeof(CardSuite)))
            {
                foreach (CardRank rankIndex in (CardRank[])Enum.GetValues(typeof(CardRank)))
                {
                    PokerCard currentCard = new PokerCard(rankIndex, suiteIndex);
                    deck.Add(currentCard);
                }
            }
        }

        public PokerCard DealOneCard()
        {
            int deckSize = deck.Count;
            int randomNumber = randomGenerator.Next(deckSize);
            PokerCard card = deck.ElementAt(randomNumber);

            return card;
        }
    }
}
