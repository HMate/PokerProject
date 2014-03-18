using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerProject.PokerGame.CardClasses;
using PokerProject.PokerGame.PlayerClasses;

namespace PokerProject.PokerGame
{
    class Table
    {
        private readonly static Table instance = new Table();
        private CardList communityCards;
        private Pot mainPot;
        private PlayerQueue players;

        public static Table Instance
        {
            get
            {
                return instance;
            }
        }

        private Table()
        {
            communityCards = new CardList();
            mainPot = new Pot();
        }

        public CardList CommunityCards
        {
            get
            {
                return communityCards;
            }
        }

        public Pot MainPot
        {
            get
            {
                return mainPot;
            }
        }

        public void ResetCommunityCards()
        {
            communityCards.Clear();
        }

        public void DealFlopCards(CardDeck deck)
        {
            communityCards.Add(deck.DealOneCard());
            communityCards.Add(deck.DealOneCard());
            communityCards.Add(deck.DealOneCard());
        }

        public void DealTurnCard(CardDeck deck)
        {
            communityCards.Add(deck.DealOneCard());
        }

        public void DealRiverCard(CardDeck deck)
        {
            communityCards.Add(deck.DealOneCard());
        }
    }
}
