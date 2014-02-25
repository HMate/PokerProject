using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerProject.PokerGame.CardClasses;

namespace PokerProject.PokerGame.PlayerClasses
{
    public interface Player
    {
        string Name
        {
            get;
        }
        int ChipCount
        {
            get;
            set;
        }
        void IncreaseChipCount(int value);
        void DecreaseChipCount(int value);
        void DrawCard(CardDeck deck);
        void DrawCard(PokerCard card);
        CardList ShowCards();
        void FoldCards();
        Player Clone();
    }
}
