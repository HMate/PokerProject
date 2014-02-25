using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerProject.PokerGame.CardClasses;

namespace PokerProject.PokerGame.PlayerClasses
{
    abstract public class BasicPlayer : Player
    {

        private CardList cards = new CardList();
        private int chips;
        private string name;

        public BasicPlayer(Player player)
        {
            chips = player.ChipCount;
            cards = new CardList(player.ShowCards());
            name = player.Name;
        }

        public BasicPlayer(string name)
        {
            this.name = name;
        }

        public BasicPlayer():this("Anonymous")
        {

        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public int ChipCount
        {
            get
            {
                return chips;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", "Negative value cannot be given to a player.");
                }
                chips = value;
            }
        }

        public void IncreaseChipCount(int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value", "Negative value cannot be given to a player.");
            }
            chips += value;
        }

        public void DecreaseChipCount(int value)
        {
            if (value > chips)
            {
                throw new ArgumentOutOfRangeException("value", "Player has less chips then the amount passed to take away.");
            }
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value", "Negative value cannot be taken away from a player.");
            }
            chips -= value;
        }

        public void DrawCard(CardDeck deck)
        {
            cards.Add(deck.DealOneCard());
        }

        public void DrawCard(PokerCard card)
        {
            PokerCard newCard = new PokerCard(card);
            cards.Add(newCard);
        }

        public CardList ShowCards()
        {
            return cards;
        }

        public void FoldCards()
        {
            cards.Clear();
        }

        public abstract Player Clone();

    }
}
