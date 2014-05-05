using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerProject.PokerGame.CardClasses
{
    public class CardDeck
    {
        public const int defaultDeckSize = 52;

        private CardList deck;
        private Random randomGenerator = new Random();

        public CardDeck()
        {
            deck = new CardList();
            createNewPokerDeck();
        }

        public void createNewPokerDeck()
        {
            deck.Clear();
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
            int currentDeckSize = deck.Count;

            if (currentDeckSize <= 0)
            {
                throw new CardDeckEmptyException("Tried to get card from an empty card deck.");
            }

            int randomNumber = randomGenerator.Next(currentDeckSize);
            PokerCard card = deck.ElementAt(randomNumber);
            deck.RemoveAt(randomNumber);
            return card;
        }

        public CardList DealCards(int amount)
        {
            CardList list = new CardList();
            for (int cardIndex = 0; cardIndex < amount; cardIndex++)
            {
                PokerCard card = DealOneCard();
                list.Add(card);
            }
            return list;
        }

        public int GetDeckSize()
        {
            return deck.Count;
        }
    }
}
