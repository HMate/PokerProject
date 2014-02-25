using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokerProject.PokerGame.PlayerClasses;
using PokerProject.PokerGame.CardClasses;

namespace PokerProject.PokerGame
{
    public class PokerAssets
    {

        List<PokerCard> communityCards = new List<PokerCard>(5);
        List<Player> players;

        public PokerAssets()
        {
            this.players = new List<Player>();
        }

        public PokerAssets(List<Player> playerList)
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

        public void SetCommunityCards(List<PokerCard> cards)
        {
            communityCards = new List<PokerCard>(cards);
        }

        public List<PokerCard> ShowCommunityCards()
        {
            return communityCards;
        }

    }
}
