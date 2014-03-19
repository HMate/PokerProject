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
        private List<Player> list = new List<Player>();
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

        public void DeletePlayer(Player player)
        {
            if (!list.Contains(player))
            {
                throw new ArgumentException("Player doesn't exist!");
            }

            String position = GetPlayerPosition(player);
            int listIndex = list.IndexOf(player);

            list.Remove(player);
            if (queue.Contains(player))
            {
                Queue<Player> tempQueue = new Queue<Player>();
                Player nextPlayer;
                while (HasNextPlayer())
                {
                    nextPlayer = GetNextPlayer();
                    if (!nextPlayer.Equals(player))
                    {
                        tempQueue.Enqueue(nextPlayer);
                    }
                }
                queue = tempQueue;
            }

            if (list.Count == 0)
            {
                dealer = null;
                smallBlind = null;
                bigBlind = null;
                return;
            }

            if (position.Equals("Dealer"))
            {
                SetDealer(list.ElementAt(listIndex));
            }
            else if (position.Equals("Small Blind"))
            {
                smallBlind = list.ElementAt(listIndex);
                bigBlind = list.ElementAt(IncrementIndex(listIndex));
            }
            else if (position.Equals("Big Blind"))
            {
                bigBlind = list.ElementAt(listIndex);
            }
        }

        public void Clear()
        {
            dealer = null;
            smallBlind = null;
            bigBlind = null;

            queue.Clear();
            list.Clear();
        }

        public List<Player> GetPlayers()
        {
            return list;
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
            int playerIndex = list.IndexOf(player);
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
            int playerIndex = list.IndexOf(player);
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
