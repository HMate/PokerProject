using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProject.PokerGame.PlayerClasses
{
    public class PlayerQueue
    {
        //TODO:
        // DeletePlayers - dealer  changing?
        private ICollection<Player> list = new List<Player>();
        private Queue<Player> queue = new Queue<Player>();
        Player dealer = null;
        Player smallBlind = null;
        Player bigBlind = null;

        public void AddPlayers(ICollection<Player> players)
        {
            foreach (Player player in players)
            {
                AddPlayer(player);
            }
        }

        public void AddPlayer(Player player)
        {
            Player newPlayer = new Player(player);
            list.Add(newPlayer);
            queue.Enqueue(newPlayer);
        }

        public List<Player> GetPlayers()
        {
            return list.ToList();
        }

        public bool HasNextPlayer()
        {
            return queue.Count != 0;
        }

        public Player GetNextPlayer()
        {
            Player nextPlayer = queue.Dequeue();
            return nextPlayer;
        }

        public Player GetNextPlayerAfterPlayer(Player player)
        {
            int playerIndex = list.ToList().IndexOf(player);
            playerIndex = IncrementIndex(playerIndex);
            return list.ElementAt(playerIndex);
        }

        public void SetPlayerFirstInOrder(Player player)
        {
            if (!list.Contains(player))
            {
                throw new ArgumentException("Player doesn't exist!");
            }

            queue.Clear();
            int playerIndex = list.ToList().IndexOf(player);
            for (int numberIndex = 0; numberIndex < list.Count; numberIndex++)
            {
                queue.Enqueue(list.ElementAt(playerIndex));
                playerIndex = IncrementIndex(playerIndex);
            }
        }

        private int IncrementIndex(int index)
        {
            return (index == list.Count - 1) ? 0 : index + 1;
        }

        public String GetPlayerPosition(Player player)
        {
            if (!list.Contains(player))
            {
                throw new ArgumentException("Player doesn't exist!");
            }

            Player dealer = GetDealer();

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

        public void SetDealer(Player player)
        {
            if (!list.Contains(player))
            {
                throw new ArgumentException("Player doesn't exist!");
            }
            dealer = player;
            smallBlind = GetNextPlayerAfterPlayer(dealer);
            bigBlind = GetNextPlayerAfterPlayer(smallBlind);
        }

        public Player GetDealer()
        {
            if (dealer == null)
            {
                throw new ArgumentException("There is no dealer.");
            }
            return dealer;
        }

        public void SetBettingOrder()
        {
            if (dealer == null)
            {
                throw new ArgumentException("There is no dealer.");
            }
            SetPlayerFirstInOrder(smallBlind);
        }

        public void SetNextHandOrder()
        {
            if (dealer == null)
            {
                throw new ArgumentException("There is no dealer.");
            }

            SetDealer(smallBlind);
            SetPlayerFirstInOrder(smallBlind);
        }

    }
}
