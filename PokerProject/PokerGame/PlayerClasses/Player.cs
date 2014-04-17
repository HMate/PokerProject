using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerProject.PokerGame.CardClasses;
using PokerProject.PokerGame.PlayerClasses.Decisions;
using PokerProject.PokerGame.PlayerClasses.PlayerAIs;

namespace PokerProject.PokerGame.PlayerClasses
{
    public class Player
    {
        
        private CardList cards;
        private int chips;
        private int betThisPhase;
        private string name;
        private PlayerController controller;
        private bool ingame = false;

        public Player(string name, PlayerController controller)
        {
            this.name = name;
            this.controller = controller;
            this.controller.SetPlayer(this);
            this.cards = new CardList();
            this.chips = 0;
            this.betThisPhase = 0;
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

        public Player(Player player)
        {
            name = player.Name;
            this.controller = player.controller.Clone();
            this.controller.SetPlayer(this);
            cards = new CardList(player.ShowCards());
            chips = player.ChipCount;
            betThisPhase = player.betThisPhase;
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

        /*
         * This is the method where the palyer decides what to do when he comes in the game.
         * The PlayerController's job, to decide this, here we just make sure that its a legal move.
         * */
        public void MakeDecision()
        {
            bool legalMove = false;

            while (!legalMove)
            {
                try
                {
                    PlayerDecision decision = controller.MakeDecision();
                    decision.ExecuteDecision();
                    if (IsLegalMove())
                    {
                        legalMove = true;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Problem during player " + name + "'s move!");
                    Console.WriteLine(e.Message+ " - " + e.Data);
                    Console.WriteLine(e.StackTrace);
                }
            }
        }

        private bool IsLegalMove()
        {
            return (betThisPhase == Table.Instance.MainPot.LargestBet || IsIngame() == false);
        }

        /*Method for posting the blind at the start of a turn.
         * First the player checks whether he is the small or the big blind.
         * Then the player posts the correct amount to the main pot.
         * */
        public void PostBlind()
        {
            Table table = Table.Instance;
            string position = table.Positions.GetPlayerPosition(this);
            int blindAmount = table.GetBigBlind();

            if (position.Equals("Small Blind"))
            {
                blindAmount /= 2;
            }
            
            if (chips < blindAmount)
            {
                blindAmount = chips;
            }

            Bet(blindAmount);
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
            betThisPhase += amount;
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
            //When player folds, he also leaves the game, so forget his bets for this turn.
            betThisPhase = 0;
            SetIngame(false);
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
