using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProject.PokerGame
{
    public class PokerCard
    {
        private CardSuite suite;
        private CardRank rank;


        public PokerCard(CardRank cardRank, CardSuite cardSuite)
        {
            rank = cardRank;
            suite = cardSuite;
        }

        public PokerCard() : this(CardRank.Ace, CardSuite.Spades)
        {
            
        }

        public CardSuite Suite
        {
            get
            {
                return suite;
            }
         }

        public CardRank Rank
        {
            get
            {
                return rank;
            }

        }


    }
}
