using PokerProject.PokerGame.CardClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerProject.PokerGame
{
    public class HandEvaluator
    {
        IList<PokerCard> savedCards;

        public CardList DetermineBestHand(CardList list)
        {
            Action<IList<PokerCard>> saveBestHand = SaveHand;
            CombinatoricsUtilities.GetCombinations<PokerCard>(list, saveBestHand, 5, false); 
            return list;
        }
        
        public void SaveHand(IList<PokerCard> cards)
        {
            if (SavedCardsAreWorseThan(cards))
            {
                savedCards = cards;
            }
        }

        private bool SavedCardsAreWorseThan(IList<PokerCard> otherCards)
        {



            return false;
        }
    }
}
