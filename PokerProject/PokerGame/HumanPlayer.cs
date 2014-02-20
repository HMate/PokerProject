using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProject.PokerGame
{
    public class HumanPlayer : Player
    {
        private List<PokerCard> cards = new List<PokerCard>(2);
        private int chips;
        private string name;

        public HumanPlayer(string name)
        {
            this.name = name;
        }

        public HumanPlayer():this("Anonymous")
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

        public void GiveChips(int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value", "Negative value cannot be given to a player.");
            }
            chips += value;
        }

        public void TakeChips(int value)
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

        public List<PokerCard> ShowCards()
        {
            return cards;
        }

        public void FoldCards()
        {
            cards.Clear();
        }
    }
}
