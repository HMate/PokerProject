using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerProject.PokerGame.PlayerClasses.Decisions;

namespace PokerProject.PokerGame.PlayerClasses.PlayerAIs
{
    class RandomAiController : PlayerController
    {
        public event Action<string> InfoChanged;

        private Player player;
        private PlayerDecision decision;
        private System.Threading.Semaphore semaphore;
        private Random randomGenerator;
        private List<string> infos;

        public RandomAiController()
        {
            semaphore = new System.Threading.Semaphore(0, 1);
            randomGenerator = new Random();
            infos = new List<string>();
        }

        public void SetPlayer(Player player)
        {
            this.player = player;
        }

        public PlayerController Clone()
        {
            return new RandomAiController();
        }

        public PlayerDecision MakeDecision()
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
                    ChangeInfo("I bet " + randBet);
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
                ChangeInfo("I fold");
            }
            else
            {
                try
                {
                    decision = new CallDecision(player);
                    ChangeInfo("I call");
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e);
                    System.Console.WriteLine(e.StackTrace);
                }
            }

            semaphore.WaitOne();
            return decision;
        }

        public PlayerDecision MakeRevealCardDecision()
        {
            decision = null;

            int rand = randomGenerator.Next(100);

            if (rand < 50)
            {
                decision = new FoldDecision(player);
                ChangeInfo("I fold");
            }
            else
            {
                decision = new ShowCardsDecision(player);
                ChangeInfo("I show my cards");
            }

            semaphore.WaitOne();
            return decision;
        }

        private void ChangeInfo(string message)
        {
            //infos.Add(message);
            if (InfoChanged != null)
            {
                InfoChanged(message);
            }
        }

        public void ApproveDecision()
        {
            try
            {
                semaphore.Release();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                System.Console.WriteLine(e.StackTrace);
            }
        }

        public void MakeBetDecision(int value)
        {
            //decision = new BetDecision(player, value);
            //semaphore.Release(1);
        }

        public void MakeFoldDecision()
        {
            //decision = new FoldDecision(player);
            //semaphore.Release(1);
        }

        public void MakeCallDecision()
        {
            //decision = new CallDecision(player);
            //semaphore.Release(1);
        }

        public void MakeShowCardsDecision()
        {
            //decision = new ShowCardsDecision(player);
            //semaphore.Release(1);
        }

        public override string ToString()
        {
            return "Random AI";
        }
    }
}
