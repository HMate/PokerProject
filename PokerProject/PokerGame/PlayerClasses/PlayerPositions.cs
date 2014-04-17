using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProject.PokerGame.PlayerClasses
{
    public class PlayerPositions
    {
        Player dealer = null;
        Player smallBlind = null;
        Player bigBlind = null;

        public void ResetPositions()
        {
            dealer = null;
            smallBlind = null;
            bigBlind = null;
        }

        /*
         * Method for setting the positions for playing the next hand
         * */
        public void SetNextHandPositions()
        {
            SetDealer(smallBlind);
        }

        public void SetDealer(Player player)
        {
            PlayerQueue players = Table.Instance.Players;
            if (!players.GetPlayers().Contains(player))
            {
                throw new ArgumentException("Player doesn't exist in the player list!");
            }
            dealer = player;
            if (IsThereOnlyOnePlayer())
            {
                return;
            }
            smallBlind = players.GetNextPlayerAfterPlayer(dealer);
            bigBlind = players.GetNextPlayerAfterPlayer(smallBlind);
        }

        public void SetSmallBlind(Player player)
        {
            PlayerQueue players = Table.Instance.Players;
            if (!players.GetPlayers().Contains(player))
            {
                throw new ArgumentException("Player doesn't exist in the player list!");
            }
            smallBlind = player;
            if (IsThereOnlyOnePlayer())
            {
                return;
            }
            bigBlind = players.GetNextPlayerAfterPlayer(smallBlind);
        }

        public void SetBigBlind(Player player)
        {
            PlayerQueue players = Table.Instance.Players;
            if (!players.GetPlayers().Contains(player))
            {
                throw new ArgumentException("Player doesn't exist in the player list!");
            }
            bigBlind = player;
        }

        private bool IsThereOnlyOnePlayer()
        {
            return (Table.Instance.Players.Count() == 1);
        }

        public Player GetDealer()
        {
            if (dealer == null)
            {
                throw new ArgumentException("There is no dealer.");
            }
            return dealer;
        }

        public Player GetSmallBlind()
        {
            if (smallBlind == null)
            {
                throw new ArgumentException("There is no small blind.");
            }
            return smallBlind;
        }

        public Player GetBigBlind()
        {
            if (bigBlind == null)
            {
                throw new ArgumentException("There is no big blind.");
            }
            return bigBlind;
        }

        public String GetPlayerPosition(Player player)
        {
            
            
            if (player.Equals(dealer))
            {
                return "Dealer";
            } 
            if (player.Equals(smallBlind))
            {
                return "Small Blind";
            }
            if (player.Equals(bigBlind))
            {
                return "Big Blind";
            }
            return "Default";
        }

    }
}
