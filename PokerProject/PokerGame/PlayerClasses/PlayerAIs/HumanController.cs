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
        public event Action<List<string>> InfoChanged;

        private Player player;
        private PlayerDecision decision;
        private System.Threading.Semaphore semaphore;

        public HumanController()
        {
            semaphore = new System.Threading.Semaphore(0, 1);
        }

        public void SetPlayer(Player player)
        {
            this.player = player;
        }

        public void SetAutomated(bool enabled)
        {
            //Human players shouldn't be automated.
        }

        public bool IsAutomated()
        {
            return false;
        }

        public PlayerController Clone()
        {
            return new HumanController();
        }

        public PlayerDecision MakeDecision()
        {
            decision = null;
            semaphore.WaitOne();
            return decision;
        }

        public PlayerDecision MakeRevealCardDecision()
        {
            decision = null;
            semaphore.WaitOne();
            return decision;
        }

        public void MakeBetDecision(int value)
        {
            decision = new BetDecision(player, value);
            semaphore.Release(1);
        }

        public void MakeFoldDecision()
        {
            decision = new FoldDecision(player);
            semaphore.Release(1);
        }

        public void MakeCallDecision()
        {
            decision = new CallDecision(player);
            semaphore.Release(1);
        }

        public void MakeShowCardsDecision()
        {
            decision = new ShowCardsDecision(player);
            semaphore.Release(1);
        }

        public void ApproveDecision()
        {
            // this method shouldn't be called for human players!
            semaphore.Release(1);
        }

        public override string ToString()
        {
            return "Human Player";
        }

    }
}
