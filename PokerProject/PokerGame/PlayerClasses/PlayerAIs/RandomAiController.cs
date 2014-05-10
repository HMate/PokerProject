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

        private PlayerDecision decision;
        private Random randomGenerator;

        public RandomAIController()
        {
            randomGenerator = new Random();
        }

        public override PlayerDecision MakeDecision()
        {
            decision = null;
            infos.Clear();

            int rand = randomGenerator.Next(100);

            if (rand < 33)
            {
                try
                {
                    int minBet = Table.Instance.MainPot.AmountToBeEligibleForPot;
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
            else if (rand < 66)
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

            int rand = randomGenerator.Next(100);

            if (rand < 50)
            {
                decision = new FoldDecision(player);
                AppendInfo("I fold");
            }
            else
            {
                decision = new ShowCardsDecision(player);
                AppendInfo("I show my cards");
            }

            SendInfo();
            if (!automated)
            {
                semaphore.WaitOne();
            }
            return decision;
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
