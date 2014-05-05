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

        //stores the amount players have spent this turn
        private Dictionary<Player, int> playerBets;



        public Pot()
        {
            playerBets = new Dictionary<Player, int>();
        }

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

        public int LargestBet
        {
            get
            {
                return largestBet;
            }
        }

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

        public void PlaceBet(Player player, int betSize)
        {
            if (betSize < largestBet && GetAmountToCall(player) != betSize)
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

        public int BetThisTurn(Player player)
        {
            int value = 0;

            if (playerBets.ContainsKey(player))
            {
                value = playerBets[player];
            }

            return value;
        }

        public int GetAmountToCall(Player player)
        {
            return AmountToBeEligibleForPot - BetThisTurn(player);
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
