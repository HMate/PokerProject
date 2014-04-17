using PokerProject.PokerGame.PlayerClasses.Decisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProject.PokerGame.PlayerClasses.PlayerAIs
{
    public class HumanController : PlayerController
    {
        Player player;

        public void SetPlayer(Player player)
        {
            this.player = player;
        }

        public PlayerController Clone()
        {
            return new HumanController();
        }

        public PlayerDecision MakeDecision()
        {
            return new BetDecision(player, 100);
        }

    }
}
