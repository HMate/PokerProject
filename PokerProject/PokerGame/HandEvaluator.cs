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
        private CardList savedCards;
        private PokerHand savedHand;

        public HandEvaluator()
        {
            savedCards = null;
            savedHand = null;
        }

        public void DetermineBestHand(CardList list)
        {
            savedCards = null;
            savedHand = null;
            CombinatoricsUtilities.GetCombinations<PokerCard>(list, SaveHand, 5, false); 
        }
        
        private void SaveHand(IList<PokerCard> cards)
        {
            CardList evaluatedCards = new CardList(cards);

            if (SavedCardsAreWorseThan(evaluatedCards))
            {
                savedCards = evaluatedCards;
                savedHand = new PokerHand(evaluatedCards);
            }
        }

        private bool SavedCardsAreWorseThan(CardList otherCards)
        {
            if (otherCards == null)
            {
                return false;
            }
            if (savedCards == null)
            {
                return true;
            }

            PokerHand otherHand = new PokerHand((CardList)otherCards);
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

        public CardList GetBestCards()
        {
            return savedCards;
        }
    }
}
