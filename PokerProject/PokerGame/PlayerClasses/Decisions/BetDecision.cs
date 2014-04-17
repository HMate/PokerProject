using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProject.PokerGame.PlayerClasses.Decisions
{
    public class BetDecision : PlayerDecision
    {
        Player player;
        int amount;

        public BetDecision(Player player, int betAmount)
        {
            this.player = player;
            this.amount = betAmount;
        }

        public void ExecuteDecision()
        {
            player.Bet(amount);
        }
    }
}
