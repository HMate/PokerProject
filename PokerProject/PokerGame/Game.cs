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

        CardList communityCards = new CardList();
        List<Player> players;

        public Game()
        {
            this.players = new List<Player>();
        }

        public Game(List<Player> playerList)
        {
            this.players = new List<Player>();
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
            players.Add(newPlayer);
        }

        public List<Player> GetPlayers()
        {
            return players;
        }

        public void SetCommunityCards(CardList cards)
        {
            communityCards = new CardList(cards);
        }

        public CardList ShowCommunityCards()
        {
            return communityCards;
        }

    }
}
