using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProject.PokerGame
{
    public class PokerCard
    {
        private CardSuite cardSuite;
        private CardRank cardRank;


        public PokerCard(CardRank rank, CardSuite suite)
        {
            cardRank = rank;
            cardSuite = suite;
        }

        public PokerCard() : this(CardRank.Ace, CardSuite.Spades)
        {
            
        }

        public CardSuite CardSuite
        {
            get
            {
                return cardSuite;
            }
         }

        public CardRank CardRank
        {
            get
            {
                return cardRank;
            }

        }


    }
}
