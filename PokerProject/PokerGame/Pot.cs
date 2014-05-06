using PokerProject.PokerGame.PlayerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerProject.PokerGame
{
    public class Pot
    {
        private int largestBet;
        private int prevLargestBet;
        private Player prevLargestBetter;

        /// <summary>
        /// Stores the amount players have spent this turn for each player.
        /// </summary>
        private Dictionary<Player, int> playerBets;

        public Pot()
        {
            playerBets = new Dictionary<Player, int>();
        }

        /// <summary>
        /// Gets the total size of the current pot.
        /// </summary>
        public int Size
        {
            get
            {
                int sum = 0;
                foreach (var pair in playerBets)
                {
                    sum += pair.Value;
                }
                return sum;
            }
        }

        /// <summary>
        /// Returns the biggest bet this turn.
        /// A valid bet has to be at least this amount.
        /// </summary>
        public int LargestBet
        {
            get
            {
                return largestBet;
            }
        }

        /// <summary>
        /// The amount of the biggest contribution to the pot in the turn.
        /// Players have to put at least this amount to the pot to be eligible to win the pot.
        /// </summary>
        public int AmountToBeEligibleForPot
        {
            get
            {
                int amount = 0;
                foreach (var pair in playerBets)
                {
                    if (amount < pair.Value)
                    {
                        amount = pair.Value;
                    }
                }

                return amount;
            }
        }

        /// <summary>
        /// A player can place the amount that he wants to bet in the pot with this method.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="betSize"></param>
        public void PlaceBet(Player player, int betSize)
        {
            if (betSize < largestBet && GetAmountToCall(player) != betSize && betSize != player.ChipCount)
            {
                throw new ArgumentException("Bet have to be at least the amount of the previous bet", "betSize");
            }
            if (largestBet <= betSize)
            {
                prevLargestBet = largestBet;
                largestBet = betSize;
                prevLargestBetter = player; // have to be here because it have to remember who was the player who bet the largest amount
            }

            int alreadyBet = 0;
            if (playerBets.ContainsKey(player))
            {
                alreadyBet = playerBets[player];
            }

            playerBets[player] = alreadyBet + betSize;
        }

        /// <summary>
        /// A player can remove moeny from the pot if he needs to.
        /// If the money removed is the amount that was the largest bet during the last betting,
        /// the largest bet will be setted back to the largest bet before that amount.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="betSize"></param>
        public void RemoveBet(Player player, int betSize)
        {
            if (player == prevLargestBetter && betSize == largestBet)
            {
                largestBet = prevLargestBet;
            }
            int alreadyBet = 0;
            if (playerBets.ContainsKey(player))
            {
                alreadyBet = playerBets[player];
            }
            else
            {
                throw new ArgumentException("Player haven't placed chips in the pot", "player");
            }

            playerBets[player] = alreadyBet - betSize;
        }

        /// <summary>
        /// Returns the amount a player contributed to the pot in this turn. 
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public int PlayerBetThisTurn(Player player)
        {
            int value = 0;

            if (playerBets.ContainsKey(player))
            {
                value = playerBets[player];
            }

            return value;
        }

        /// <summary>
        /// Returns the amount the given player have to place in the pot to be eligible for the pot.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public int GetAmountToCall(Player player)
        {
            return AmountToBeEligibleForPot - PlayerBetThisTurn(player);
        }

        /// <summary>
        /// Empties out the pot.
        /// </summary>
        public void Empty()
        {
            playerBets.Clear();
            prevLargestBetter = null;
            largestBet = 0;
            prevLargestBet = 0;
        }
    }
}
