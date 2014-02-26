using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerProject.PokerGame
{
    public class Pot
    {
        private int money;

        public int Size
        {
            get
            {
                return money;
            }
        }

        public void PlaceBet(int betSize)
        {
            money += betSize;
        }
    }
}
