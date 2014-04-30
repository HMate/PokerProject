using PokerProject.PokerGame.CardClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProject.PokerGame
{
    class PokerHand
    {
        public PokerHand(CardList cards)
        {

        }

        public enum Hand
        {
            HighCard = 0,
            Pair = 1,
            TwoPair = 2,
            ThreeOfAKind = 3,
            Straight = 4,
            Flush = 5,
            FullHouse = 6,
            Poker = 7,
            RoyalFlush = 8
        }
    }
}
