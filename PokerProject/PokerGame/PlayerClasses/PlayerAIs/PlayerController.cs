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
        event Action<List<string>> InfoChanged;

        PlayerDecision MakeDecision();
        PlayerDecision MakeRevealCardDecision();

        void MakeBetDecision(int value);
        void MakeFoldDecision();
        void MakeCallDecision();
        void MakeShowCardsDecision();
        void ApproveDecision();

        void SetAutomated(bool enabled);
        bool IsAutomated();

        PlayerController Clone();
        void SetPlayer(Player player);
    }
}
