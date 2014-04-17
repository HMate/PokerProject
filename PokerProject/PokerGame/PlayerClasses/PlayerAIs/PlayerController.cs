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
        PlayerController Clone();
        void SetPlayer(Player player);
    }
}
