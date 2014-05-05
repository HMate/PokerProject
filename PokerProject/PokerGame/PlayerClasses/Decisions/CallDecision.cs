using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProject.PokerGame.PlayerClasses.Decisions
{
    class CallDecision : PlayerDecision
    {
        Player player;
        int amount;

        public CallDecision(Player player)
        {
            this.player = player;
            Pot mainPot = PokerGame.Table.Instance.MainPot;
            this.amount = mainPot.GetAmountToCall(player);
        }

        public void ExecuteDecision()
        {
            player.Bet(amount);
        }
    }
}
