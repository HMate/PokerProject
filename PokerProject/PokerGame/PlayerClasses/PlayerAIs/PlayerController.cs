using PokerProject.PokerGame.PlayerClasses.Decisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProject.PokerGame.PlayerClasses.PlayerAIs
{
    public interface PlayerController
    {
        PlayerDecision MakeDecision();
        PlayerDecision MakeRevealCardDecision();
        void MakeBetDecision(int value);
        void MakeFoldDecision();
        void MakeCallDecision();
        void MakeShowCardsDecision();
        PlayerController Clone();
        void SetPlayer(Player player);
    }
}
