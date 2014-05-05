using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProject.PokerGame.PlayerClasses.Decisions
{
    class FoldDecision : PlayerDecision
    {
        Player player;

        public FoldDecision(Player player)
        {
            this.player = player;
        }

        public void ExecuteDecision()
        {
            player.FoldCards();
        }
    }
}
