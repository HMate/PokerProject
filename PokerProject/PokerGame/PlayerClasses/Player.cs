using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerProject.PokerGame.CardClasses;

namespace PokerProject.PokerGame.PlayerClasses
{
    public class Player
    {
        
        private CardList cards;
        private int chips;
        private string name;
        private PlayerController controller;
        private bool ingame = false;

        public Player(Player player)
        {
            chips = player.ChipCount;
            cards = new CardList(player.ShowCards());
            name = player.Name;
            this.controller = player.controller;
        }

        public Player(string name, PlayerController controller)
        {
            this.name = name;
            this.controller = controller;
            this.cards = new CardList();
        }

        public Player(PlayerController controller) :this("Anonymous", controller)
        {

        }

        public Player(string name) :this(name,new HumanController())
        {

        }

        public Player() :this("Anonymous",new HumanController())
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
                    throw new ArgumentOutOfRangeException("value",
                        "Negative value cannot be given to a player.");
                }
                chips = value;
            }
        }

        /*Method for posting the blind at the start of a turn.
         * First the player checks whether he is the small or the big blind.
         * Then the palyer posts the correct amount to the main pot.
         * 
         * */
        public void PostBlind()
        {
            Table table = Table.Instance;
            string position = table.Positions.GetPlayerPosition(this);
            int blindAmount = table.GetBlind();

            if (position.Equals("Small Blind"))
            {
                blindAmount /= 2;
            }

            Pot mainPot = Table.Instance.MainPot;
            if (chips < blindAmount)
            {
                blindAmount = chips;
            }
            mainPot.PlaceBet(blindAmount);
            DecreaseChipCount(blindAmount);
        }

        /*
         * Method used to Bet an amount to the main pot.
         * If the player dont have enough chips,
         * then he doesn't add any chips to the pot.
         * */
        public void Bet(int amount)
        {
            Pot mainPot = Table.Instance.MainPot;
            DecreaseChipCount(amount);
            mainPot.PlaceBet(amount);
        }

        public void DecreaseChipCount(int value)
        {
            if (value > chips)
            {
                throw new ArgumentOutOfRangeException("value",
                    "Player has less chips then the amount passed to take away.");
            }
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value",
                    "Negative value cannot be taken away from a player.");
            }
            chips -= value;
        }


        public void IncreaseChipCount(int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value",
                    "Negative value cannot be given to a player.");
            }
            chips += value;
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

        public Player Clone()
        {
            return new Player(this);
        }

        public void SetIngame(bool ingame)
        {
            this.ingame = ingame;
        }

        public bool IsIngame()
        {
            return ingame;
        }
    }
}
