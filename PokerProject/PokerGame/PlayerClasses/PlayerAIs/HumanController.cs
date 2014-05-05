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
        PlayerDecision decision;
        System.Threading.Semaphore semaphore;

        public HumanController()
        {
            semaphore = new System.Threading.Semaphore(1,1);
        }

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
            decision = null;
            semaphore.WaitOne();
            return decision;
        }

        public void MakeBetDecision(int value)
        {
            decision = new BetDecision(player, value);
            semaphore.Release();
        }

        public void MakeFoldDecision()
        {
            decision = new FoldDecision(player);
            semaphore.Release();
        }

        public void MakeCallDecision()
        {
            decision = new CallDecision(player);
            semaphore.Release();
        }

        public override string ToString()
        {
            return "Human Player";
        }

    }
}
