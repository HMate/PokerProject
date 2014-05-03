using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerProject.PokerGame
{
    public class Pot
    {
        private int money;
        private int largestBet;

        public int Size
        {
            get
            {
                return money;
            }
        }

        public int LargestBet
        {
            get
            {
                return largestBet;
            }
        }

        public void PlaceBet(int betSize)
        {
            if (largestBet < betSize)
            {
                largestBet = betSize;
            }
            money += betSize;
        }

        /// <summary>
        /// Empties out the pot.
        /// </summary>
        public void Empty()
        {
            money = 0;
            largestBet = 0;
        }
    }
}
