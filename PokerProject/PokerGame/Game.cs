using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokerProject.PokerGame.PlayerClasses;
using PokerProject.PokerGame.CardClasses;

namespace PokerProject.PokerGame
{
    public class Game
    {

        Table table;
        CardDeck deck;

        public Game()
        {
            table = Table.Instance;
            this.deck = new CardDeck();
        }

        /*
         * Main game states happen here
         * */
        public void MainGameTurn()
        {
            DealCards();
            PlaceBlinds();
            /*
             * 
             * 
             * 
             * 
             * */
        }


        public void DealCards()
        {
            PlayerQueue players = table.Players;
            players.SetBettingOrder();
            while (players.HasNextPlayer())
            {
                Player player = players.GetNextPlayer();
                player.DrawCard(deck);
                player.DrawCard(deck);
            }
        }

        public void PlaceBlinds()
        {
            PlayerQueue players = table.Players;
            players.SetBettingOrder();

            if (players.HasNextPlayer())
            {
                Player smallBlind = players.GetNextPlayer();
                /*smallBlind.*/
            }
            while (players.HasNextPlayer())
            {
                Player player = players.GetNextPlayer();
            }
        }




    }
}
