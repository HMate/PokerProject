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

        public SimpleAIController()
        {

        }

        protected override void MakeAIDecision()
        {
            decision = new CallDecision(player);
            AppendInfo("I call");
        }

        protected override void MakeRevealCardAIDecision()
        {
            decision = new ShowCardsDecision(player);
            AppendInfo("I show my cards");
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
