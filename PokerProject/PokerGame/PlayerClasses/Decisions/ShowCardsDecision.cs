using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProject.PokerGame.PlayerClasses.Decisions
{
    class ShowCardsDecision :PlayerDecision
    {
        Player player;

        public ShowCardsDecision(Player player)
        {
            this.player = player;
        }

        public void ExecuteDecision()
        {
            player.RevealCards = true;
        }
    }
}
