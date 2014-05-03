using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerProject.PokerGame.CardClasses;
using PokerProject.PokerGame.PlayerClasses;

namespace PokerProject.PokerGame
{
    public class Table
    {
        private readonly static Table instance = new Table();
        private CardList communityCards;
        private Pot mainPot;
        private PlayerQueue players;
        private PlayerPositions positions;
        private int blindAmount;

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
            players = new PlayerQueue();
            positions = new PlayerPositions();
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

        public PlayerQueue Players
        {
            get
            {
                return players;
            }
        }

        public PlayerPositions Positions
        {
            get
            {
                return positions;
            }
        }

        /// <summary>
        /// Clears the community card pool.
        /// </summary>
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


        public void SetBlind(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException("amount", amount, "Blind cannot be negative.");
            }
            blindAmount = amount;
        }

        public int GetBigBlind()
        {
            return blindAmount;
        }

        public int GetSmallBlind()
        {
            return blindAmount / 2;
        }

        /// <summary>
        /// Resets the Blind to 0.
        /// </summary>
        public void ResetBlind()
        {
            blindAmount = 0;
        }
    }
}
