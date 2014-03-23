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

            PlayerPositions positions = Table.Instance.Positions;
            String position = positions.GetPlayerPosition(player);
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
                return;
            }

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

        public void Clear()
        {
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

        public void SetBettingOrder()
        {
            SetPlayerFirstInOrder(Table.Instance.Positions.GetSmallBlind());
        }

        public void SetNextHandOrder()
        {
            PlayerPositions positions = Table.Instance.Positions;
            positions.SetDealer(positions.GetSmallBlind());
            SetPlayerFirstInOrder(positions.GetSmallBlind());
        }

    }
}
