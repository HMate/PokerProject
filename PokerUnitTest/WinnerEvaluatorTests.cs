using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using PokerProject.PokerGame;
using PokerProject.PokerGame.PlayerClasses;
using PokerProject.PokerGame.CardClasses;

namespace PokerUnitTest
{
    [TestClass]
    public class WinnerEvaluatorTests
    {
        GameWinnerEvaluator evaluator;
        Table table;
        PlayerQueue testPlayers;
        System.Collections.Generic.List<Player> playerList;

        [TestInitialize]
        public void SetUp()
        {
            evaluator = new GameWinnerEvaluator();

            table = Table.Instance;

            //initialize players
            testPlayers = table.Players;
            testPlayers.Clear();

            testPlayers.AddPlayer(new Player("Jack"));
            testPlayers.AddPlayer(new Player("Jill"));
            testPlayers.AddPlayer(new Player("Bob"));
            testPlayers.AddPlayer(new Player("Albert"));
            testPlayers.AddPlayer(new Player("Dennis"));
            testPlayers.AddPlayer(new Player("Barbara"));
            testPlayers.AddPlayer(new Player("Carla"));

            playerList = testPlayers.GetPlayersList();
        }


        /*
         * Test if the evaluator can decide who is the winner when he have a pair, and everybody else has nothing.
         * */
        [TestMethod]
        public void WinnerTest()
        {
            //comm cards [H]4, [H]5, [H]7, [D]9, [S]K
            CardList communityCards = table.CommunityCards;
            communityCards.Add(new PokerCard(CardRank.Four, CardSuite.Hearts));
            communityCards.Add(new PokerCard(CardRank.Five, CardSuite.Hearts));
            communityCards.Add(new PokerCard(CardRank.Seven, CardSuite.Hearts));
            communityCards.Add(new PokerCard(CardRank.Nine, CardSuite.Diamonds));
            communityCards.Add(new PokerCard(CardRank.King, CardSuite.Spades));

            //winner have: [C]A,[D]A
            Player expectedWinner = playerList.ElementAt(0);
            expectedWinner.SetIngame(true);
            expectedWinner.DrawCard(new PokerCard(CardRank.Ace, CardSuite.Clubs));
            expectedWinner.DrawCard(new PokerCard(CardRank.Ace, CardSuite.Diamonds));

            //loser have: [C]10, [D]2
            Player expectedLoser = playerList.ElementAt(1);
            expectedLoser.SetIngame(true);
            expectedLoser.DrawCard(new PokerCard(CardRank.Ten, CardSuite.Clubs));
            expectedLoser.DrawCard(new PokerCard(CardRank.Two, CardSuite.Diamonds));

            for (int i = 2; i < playerList.Count; i++)
            {
                playerList[i].SetIngame(false);
            }

            Player winner = evaluator.DetermineWinner();

            Assert.AreSame(expectedWinner, winner);
        }
    }
}
