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
        private IList<PokerCard> savedCards;
        private PokerHand savedHand;

        public HandEvaluator()
        {
            savedCards = null;
            savedHand = null;
        }

        public void DetermineBestHand(IList<PokerCard> list)
        {
            savedCards = null;
            savedHand = null;
            CombinatoricsUtilities.GetCombinations<PokerCard>(list, SaveHand, 5, false); 
        }
        
        private void SaveHand(IList<PokerCard> cards)
        {
            if (SavedCardsAreWorseThan(cards))
            {
                savedHand = new PokerHand(cards.ToCardList());
                savedCards = cards.ToCardList();
            }
        }

        private bool SavedCardsAreWorseThan(IList<PokerCard> otherCards)
        {
            if (otherCards == null)
            {
                return false;
            }
            if (savedCards == null)
            {
                return true;
            }

            PokerHand otherHand = new PokerHand(otherCards.ToCardList());
            if (savedHand < otherHand)
            {
                return true;
            }

            return false;
        }

        public PokerHand GetBestHand()
        {
            return savedHand;
        }

        public IList<PokerCard> GetBestCards()
        {
            return savedCards;
        }
    }
}
