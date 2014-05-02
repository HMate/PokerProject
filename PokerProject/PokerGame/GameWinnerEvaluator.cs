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
        public Player DetermineWinner()
        {
            CardList communityCards = Table.Instance.CommunityCards;

            foreach (Player player in Table.Instance.Players.GetPlayersList())
            {
                CardList playerCards = player.ShowCards();
            }

            return new Player("Jack");
        }

    }
}
