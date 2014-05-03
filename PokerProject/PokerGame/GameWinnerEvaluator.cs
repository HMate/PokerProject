using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokerProject.PokerGame.PlayerClasses;
using PokerProject.PokerGame.CardClasses;

namespace PokerProject.PokerGame
{
    public class GameWinnerEvaluator
    {
        private Player winner = null;
        private List<Player> winners = null;
        private PokerHand winnerHand = null;

        private HandEvaluator evaluator = new HandEvaluator();
        private bool isTied = false;

        public void DetermineWinner()
        {
            CardList communityCards = Table.Instance.CommunityCards;
            winners = new List<Player>();

            foreach (Player player in Table.Instance.Players.GetPlayersList())
            {
                if (player.IsIngame())
                {
                    CardList playerCards = player.ShowCards();

                    CardList possiblePlayerCards = new CardList(communityCards);
                    possiblePlayerCards.AddRange(playerCards);

                    evaluator.DetermineBestHand(possiblePlayerCards);
                    PokerHand playerBestHand = evaluator.GetBestHand();

                    if (winnerHand == null || winnerHand < playerBestHand)
                    {
                        if (winners.Count > 0)
                        {
                            isTied = false;
                            winners.Clear();
                        }
                        winner = player;
                        winnerHand = playerBestHand;
                        winners.Add(player);
                    }
                    else if (winnerHand == playerBestHand)
                    {
                        isTied = true;
                        winners.Add(player);
                    }
                }
            }
        }

        public Player Winner
        {
            get
            {
                return winner;
            }
        }

        public PokerHand WinnerHand
        {
            get
            {
                return winnerHand;
            }
        }

        public bool IsTied()
        {
            return isTied;
        }

        public List<Player> GetTiedWinners()
        {
            return winners;
        }

    }
}
