using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerProject.PokerGame.PlayerClasses.Decisions;

namespace PokerProject.PokerGame.PlayerClasses.PlayerAIs
{
    class RandomAIController : AbstractAIController
    {
        private Random randomGenerator;

        public RandomAIController()
        {
            randomGenerator = new Random();
        }

        protected override void MakeAIDecision()
        {
            int rand = randomGenerator.Next(100);

            if (rand < 33)
            {
                try
                {
                    int minBet = Table.Instance.MainPot.AmountToBeEligibleForPot + Table.Instance.MainPot.LargestBet;
                    int atMax = player.ChipCount;
                    int atLeast = (player.ChipCount < minBet) ? player.ChipCount : minBet;

                    int randBet = randomGenerator.Next(atLeast, atMax);

                    decision = new BetDecision(player, randBet);
                    AppendInfo("I bet " + randBet);
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e);
                    System.Console.WriteLine(e.StackTrace);
                }
                
            }
            else if (rand < 43)
            {
                decision = new FoldDecision(player);
                AppendInfo("I fold");
            }
            else
            {
                try
                {
                    decision = new CallDecision(player);
                    AppendInfo("I call");
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e);
                    System.Console.WriteLine(e.StackTrace);
                }
            }
        }

        protected override void MakeRevealCardAIDecision()
        {
            int rand = randomGenerator.Next(100);

            if (rand < 0)
            {
                decision = new FoldDecision(player);
                AppendInfo("I fold");
            }
            else
            {
                decision = new ShowCardsDecision(player);
                AppendInfo("I show my cards");
            }
        }

        public override string ToString()
        {
            return "Random AI";
        }

        public override PlayerController Clone()
        {
            return new RandomAIController();
        }
    }
}
