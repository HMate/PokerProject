using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerProject.PokerGame.PlayerClasses.Decisions;

namespace PokerProject.PokerGame.PlayerClasses.PlayerAIs
{
    class SimpleAIController : AbstractAIController
    {
        private PlayerDecision decision;

        public SimpleAIController()
        {

        }

        public override PlayerDecision MakeDecision()
        {
            decision = null;
            infos.Clear();

            //int minBet = Table.Instance.MainPot.AmountToBeEligibleForPot;
            //int atMax = player.ChipCount;
            //int atLeast = (player.ChipCount < minBet) ? player.ChipCount : minBet;
                
            decision = new CallDecision(player);
            AppendInfo("I call");
            SendInfo();

            if (!automated)
            {
                semaphore.WaitOne();  
            }
            
            return decision;
        }

        public override PlayerDecision MakeRevealCardDecision()
        {
            decision = null;

            decision = new ShowCardsDecision(player);
            AppendInfo("I show my cards");
            SendInfo();

            if (!automated)
            {
                semaphore.WaitOne();
            }
            return decision;
        }

        public override string ToString()
        {
            return "Simple AI";
        }

        public override PlayerController Clone()
        {
            return new SimpleAIController();
        }
    }
}
