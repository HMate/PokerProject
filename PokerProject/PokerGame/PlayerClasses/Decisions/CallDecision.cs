﻿using System;
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
            this.amount = (player.ChipCount > mainPot.GetAmountToCall(player)) ? mainPot.GetAmountToCall(player) : player.ChipCount;
        }

        public void ExecuteDecision()
        {
            player.Bet(amount);
        }
    }
}
