using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProject.PokerGame
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
        void GiveChips(int value);
        void TakeChips(int value);
        void DrawCard(CardDeck deck);
        List<PokerCard> ShowCards();
        void FoldCards();
    }
}
