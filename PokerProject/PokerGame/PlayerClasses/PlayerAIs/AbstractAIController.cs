using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerProject.PokerGame.PlayerClasses.Decisions;

namespace PokerProject.PokerGame.PlayerClasses.PlayerAIs
{
    abstract class AbstractAIController : PlayerController
    {
        public event Action<List<string>> InfoChanged;

        protected Player player;
        protected System.Threading.Semaphore semaphore;
        protected List<string> infos;
        protected bool automated;

        public AbstractAIController()
        {
            semaphore = new System.Threading.Semaphore(0, 1);
            infos = new List<string>();
            automated = false;
        }

        public void SetPlayer(Player player)
        {
            this.player = player;
        }

        public void SetAutomated(bool enabled)
        {
            automated = enabled;
        }

        public bool IsAutomated()
        {
            return automated;
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

        protected void AppendInfo(string message)
        {
            infos.Add(message);
        }

        protected void SendInfo()
        {
            if (!automated && InfoChanged != null)
            {
                InfoChanged(infos);
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

        public abstract PlayerDecision MakeDecision();
        public abstract PlayerDecision MakeRevealCardDecision();
        public abstract PlayerController Clone();
    }
}
