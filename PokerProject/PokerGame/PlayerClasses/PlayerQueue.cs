using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProject.PokerGame.PlayerClasses
{
    public class PlayerQueue
    {
        // The list of the players
        private List<Player> list = new List<Player>();

        // The order in which the players come in the game
        private Queue<Player> queue = new Queue<Player>();

        public void AddPlayers(ICollection<Player> players)
        {
            foreach (Player player in players)
            {
                AddPlayer(player);
            }
        }

        /*
         * Adding players to the list.Makes a deep copy.
         * New players are automatically added to the queue as well.
         * */
        public void AddPlayer(Player player)
        {
            Player newPlayer = new Player(player);
            list.Add(newPlayer);
            queue.Enqueue(newPlayer);
        }

        /*
         * Delets a player from the queue
         * 
         * */
        public void DeletePlayer(Player player)
        {
            CheckIfListContainsPlayer(player);

            //Remember the players position.
            // If the player had a special position, we have to set it for the next player after removing the current player.
            PlayerPositions positions = Table.Instance.Positions;
            String position = positions.GetPlayerPosition(player);
            int listIndex = list.IndexOf(player);

            //remove from list
            list.Remove(player);
            //remove from queue
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

            //if no player left in game, we don't have to set any positions. 
            if (list.Count == 0)
            {
                return;
            }

            //Set the players position who comes after the deleted player.
            if (position.Equals("Dealer"))
            {
                positions.SetDealer(list.ElementAt(listIndex));
            }
            else if (position.Equals("Small Blind"))
            {
                positions.SetSmallBlind(list.ElementAt(listIndex));
            }
            else if (position.Equals("Big Blind"))
            {
                positions.SetBigBlind(list.ElementAt(listIndex));
            }
        }

        /*
         * To see who comes after a player.
         * */
        public Player GetNextPlayerAfterPlayer(Player player)
        {
            CheckIfListContainsPlayer(player);

            int playerIndex = list.IndexOf(player);
            playerIndex = IncrementIndex(playerIndex);
            return list.ElementAt(playerIndex);
        }

        /*
         * The first player who always do something in every phase is the small blind
         * */
        public void SetBettingOrder()
        {
            SetPlayerFirstInOrder(Table.Instance.Positions.GetSmallBlind());
        }

        /*
         * Sets player first in the queue
         * */
        public void SetPlayerFirstInOrder(Player player)
        {
            CheckIfListContainsPlayer(player);

            queue.Clear();
            int playerIndex = list.IndexOf(player);
            for (int numberIndex = 0; numberIndex < list.Count; numberIndex++)
            {
                queue.Enqueue(list.ElementAt(playerIndex));
                playerIndex = IncrementIndex(playerIndex);
            }
        }

        private void CheckIfListContainsPlayer(Player player)
        {
            if (!list.Contains(player))
            {
                throw new ArgumentException("Player doesn't exist!");
            }
        }

        private int IncrementIndex(int index)
        {
            return (index == list.Count - 1) ? 0 : index + 1;
        }

        public void Clear()
        {
            queue.Clear();
            list.Clear();
        }

        public int Count()
        {
            return list.Count;
        }

        public List<Player> GetPlayersList()
        {
            return list;
        }

        public Player GetFirstPlayer()
        {
            if (list.Count > 0)
            {
                return list.ElementAt(0);
            }
            else
            {
                return null;
            }
            
        }

        public bool HasNextPlayer()
        {
            return queue.Count != 0;
        }

        public Player GetNextPlayer()
        {
            return queue.Dequeue();
        }

    }
}
