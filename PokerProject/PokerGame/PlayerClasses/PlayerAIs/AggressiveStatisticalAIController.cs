using PokerProject.PokerGame.PlayerClasses.Decisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProject.PokerGame.PlayerClasses.PlayerAIs
{
    class AggressiveStatisticalAIController : StatisticalAIController
    {
        /// <summary>
        /// Decides what to do preFlop, based on the playabilityRatio
        /// Plays aggressively
        /// </summary>
        /// <param name="ratio"></param>
        protected override void DecisionByRatio(double ratio)
        {
            var mainPot = table.MainPot;
            if (ratio > 0.75)
            {
                if (player.ChipCount < mainPot.TotalSize / 20)
                {
                    int allIn = player.ChipCount;
                    decision = new BetDecision(player, allIn);
                    AppendInfo("I have few chips left and a good hand, so I all in");
                }
                else if (mainPot.GetAmountToCall(player) <= player.ChipCount / 3)
                {
                    int randBet = MakeRandomBet();
                    decision = new BetDecision(player, randBet);
                    AppendInfo("hand is good so i raise " + randBet + "$");
                }
                else
                {
                    decision = new CallDecision(player);
                    AppendInfo("hand is good so I call.");
                }
            }
            else if (ratio > 0.33)
            {
                if (table.MainPot.GetAmountToCall(player) <= player.ChipCount / 3)
                {
                    decision = new CallDecision(player);
                    AppendInfo("hand is decent so i call");
                }
                else
                {
                    decision = new FoldDecision(player);
                    AppendInfo("hand not good enough to call, so I fold");
                }
            }
            else
            {
                if (mainPot.GetAmountToCall(player) <= player.ChipCount / 20)
                {
                    decision = new CallDecision(player);
                    AppendInfo("have to call little, so i call");
                }
                else
                {
                    decision = new FoldDecision(player);
                    AppendInfo("hand is bad, I fold");
                }
            }
        }

        /// <summary>
        /// Decides what to do in current turn, based on the HandStrength
        /// Plays aggressively
        /// </summary>
        protected override void DecisionByCardStrength()
        {
            AppendInfo("Hand Strength: " + handStrength);
            if (handStrength == HandStrength.VeryGood || handStrength == HandStrength.Good || handStrength == HandStrength.LikelyDraw)
            {
                if (player.ChipCount < table.MainPot.TotalSize / 20)
                {
                    int allIn = player.ChipCount;
                    decision = new BetDecision(player, allIn);
                    AppendInfo("I have few chips left and good a hand, so I all in");
                }
                else 
                {
                    int randBet = MakeRandomBet();
                    decision = new BetDecision(player, randBet);
                    AppendInfo("I bet " + randBet + "$");
                }
            }
            else if ((handStrength == HandStrength.Bad || handStrength == HandStrength.VeryBad)
                && table.MainPot.GetAmountToCall(player) > player.ChipCount / 10)
            {
                decision = new FoldDecision(player);
                AppendInfo("I fold");
            }
            else
            {
                decision = new CallDecision(player);
                AppendInfo("I call");
            }
        }

        public override string ToString()
        {
            return "Aggressive Statistical AI";
        }
    }
}
