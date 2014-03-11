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
        //TODO:
        //több mint 8 játékos

        CardList communityCards;
        List<Player> playerQueue;
        Pot mainPot;
        CardDeck deck;

        public Game()
        {
            this.communityCards = new CardList();
            this.playerQueue = new List<Player>();
            this.mainPot = new Pot();
            this.deck = new CardDeck();
        }

        public Game(List<Player> playerList)
        {
            this.playerQueue = new List<Player>();
            AddPlayers(playerList);
        }

        public void AddPlayers(List<Player> playerList)
        {
            foreach (Player player in playerList)
            {
                AddPlayer(player);
            }
        }

        public void AddPlayer(Player player)
        {
            Player newPlayer = player.Clone();
            playerQueue.Add(newPlayer);
        }

        public List<Player> GetPlayers()
        {
            return playerQueue;
        }

        public void SetCommunityCards(CardList cards)
        {
            communityCards = new CardList(cards);
        }

        public CardList ShowCommunityCards()
        {
            return communityCards;
        }

        /*
         * Main game states hapen here
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
             * 
             * 
             * 
             * 
             * 
             * 
             * 
             * */
        }


        public void DealCards()
        {
            foreach (Player player in playerQueue)
            {
                player.DrawCard(deck);
                player.DrawCard(deck);
            }
        }

        public void PlaceBlinds()
        {
            foreach (Player player in playerQueue)
            {
                player;
            }
        }




    }
}
