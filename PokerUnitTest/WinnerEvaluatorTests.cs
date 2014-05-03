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
            //initilize table
            table = Table.Instance;
            table.ResetCommunityCards();

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
            testPlayers.AddPlayer(new Player("Sullivan"));

            playerList = testPlayers.GetPlayersList();
        }


        /*
         * Test if the evaluator can decide who is the winner when he have a pair, and everybody else has nothing.
         * */
        [TestMethod]
        public void WinnerTwoPlayerTest()
        {
            //community cards:
            CardList communityCards = table.CommunityCards;
            communityCards.AddRange(MakeCardList("4H,5H,7H,9D,KS"));

            //winner player:
            Player expectedWinner = playerList.ElementAt(0);
            expectedWinner.SetIngame(true);
            DealCardsToPlayer(expectedWinner, "AC,AD");

            //loser player:
            Player expectedLoser = playerList.ElementAt(1);
            expectedLoser.SetIngame(true);
            DealCardsToPlayer(expectedLoser, "TC,2D");

            for (int i = 2; i < playerList.Count; i++)
            {
                playerList[i].SetIngame(false);
            }

            evaluator.DetermineWinner();

            Assert.IsFalse(evaluator.IsTied());

            Player winner = evaluator.Winner;

            Assert.AreEqual(PokerHand.HandCategory.Pair, evaluator.WinnerHand.Category);
            Assert.AreEqual(CardRank.Ace, evaluator.WinnerHand.Rank);
            Assert.AreSame(expectedWinner, winner, "expected: {0}, actual: {1}", expectedWinner.Name, winner.Name);
        }

        [TestMethod]
        public void WinnerEightPlayerTest()
        {
            //community cards:
            CardList communityCards = table.CommunityCards;
            communityCards.AddRange(MakeCardList("2C,AS,AD,AC,9D"));
            
            //1st player:
            Player firstPlayer = playerList.ElementAt(0);
            DealCardsToPlayer(firstPlayer, "JS,7D");

            //2nd player:
            Player secondPlayer = playerList.ElementAt(1);
            DealCardsToPlayer(secondPlayer, "4C,QH");

            //3rd player:
            Player thirdPlayer = playerList.ElementAt(2);
            DealCardsToPlayer(thirdPlayer, "TS,6S");

            //4th player:
            Player fourthPlayer = playerList.ElementAt(3);
            DealCardsToPlayer(fourthPlayer, "JH,JD");

            //5th player:
            Player fifthPlayer = playerList.ElementAt(4);
            DealCardsToPlayer(fifthPlayer, "2S,QC");

            //6th player:
            Player sixthPlayer = playerList.ElementAt(5);
            DealCardsToPlayer(sixthPlayer, "3C,7C");

            //7th player: he should win with Ace Poker
            Player seventhPlayer = playerList.ElementAt(6);
            DealCardsToPlayer(seventhPlayer, "AH,QS");

            //8th player:
            Player eighthPlayer = playerList.ElementAt(7);
            DealCardsToPlayer(eighthPlayer, "5H,9S");
            
            for (int i = 0; i < playerList.Count; i++)
            {
                playerList[i].SetIngame(true);
            }

            evaluator.DetermineWinner();

            Assert.IsFalse(evaluator.IsTied());

            Player winner = evaluator.Winner;

            Assert.AreEqual(PokerHand.HandCategory.Poker, evaluator.WinnerHand.Category);
            Assert.AreEqual(CardRank.Ace, evaluator.WinnerHand.Rank);
            Assert.AreSame(seventhPlayer, winner, "expected: {0}, actual: {1}", seventhPlayer.Name, winner.Name);
        }

        /*
         * Test when players have tie:
         * Both players play with the community cards, which gives them a straight
         * */
        [TestMethod]
        public void WinnerTwoPlayerTieTest()
        {
            //community cards:
            CardList communityCards = table.CommunityCards;
            communityCards.AddRange(MakeCardList("4H,5H,6H,7D,8S"));

            //winner player:
            Player firstPlayer = playerList.ElementAt(0);
            firstPlayer.SetIngame(true);
            DealCardsToPlayer(firstPlayer, "AC,AD");

            //loser player:
            Player secondPlayer = playerList.ElementAt(1);
            secondPlayer.SetIngame(true);
            DealCardsToPlayer(secondPlayer, "TC,2D");

            for (int i = 2; i < playerList.Count; i++)
            {
                playerList[i].SetIngame(false);
            }

            evaluator.DetermineWinner();
            Assert.IsTrue(evaluator.IsTied());

            List<Player> winners = evaluator.GetTiedWinners();

            Assert.AreEqual(PokerHand.HandCategory.Straight, evaluator.WinnerHand.Category);
            Assert.AreEqual(CardRank.Eight, evaluator.WinnerHand.Rank);
            Assert.AreEqual(2, winners.Count);
            CollectionAssert.Contains(winners, firstPlayer, "First player should be in winners");
            CollectionAssert.Contains(winners, secondPlayer, "Second player should be in winners");
        }

        /*
         * Test when players have tie with 8 players:
         * Both players play with the community cards, which gives them a straight
         * */
        [TestMethod]
        public void WinnerEightPlayerTieTest()
        {
            //community cards:
            CardList communityCards = table.CommunityCards;
            communityCards.AddRange(MakeCardList("2C,AS,AD,AC,9D"));

            //1st player:
            Player firstPlayer = playerList.ElementAt(0);
            DealCardsToPlayer(firstPlayer, "JS,7D");

            //2nd player: should be a tied winner
            Player secondPlayer = playerList.ElementAt(1);
            DealCardsToPlayer(secondPlayer, "9H,QH");

            //3rd player:
            Player thirdPlayer = playerList.ElementAt(2);
            DealCardsToPlayer(thirdPlayer, "TS,6S");

            //4th player: should be a tied winner
            Player fourthPlayer = playerList.ElementAt(3);
            DealCardsToPlayer(fourthPlayer, "JH,9C");

            //5th player:
            Player fifthPlayer = playerList.ElementAt(4);
            DealCardsToPlayer(fifthPlayer, "2S,QC");

            //6th player:
            Player sixthPlayer = playerList.ElementAt(5);
            DealCardsToPlayer(sixthPlayer, "3C,7C");

            //7th player:
            Player seventhPlayer = playerList.ElementAt(6);
            DealCardsToPlayer(seventhPlayer, "7H,QS");

            //8th player: should be a tied winner
            Player eighthPlayer = playerList.ElementAt(7);
            DealCardsToPlayer(eighthPlayer, "5H,9S");

            for (int i = 0; i < playerList.Count; i++)
            {
                playerList[i].SetIngame(true);
            }

            evaluator.DetermineWinner();
            Assert.IsTrue(evaluator.IsTied());

            List<Player> winners = evaluator.GetTiedWinners();

            Assert.AreEqual(PokerHand.HandCategory.FullHouse, evaluator.WinnerHand.Category);
            Assert.AreEqual(CardRank.Ace, evaluator.WinnerHand.Rank);
            Assert.AreEqual(CardRank.Nine, evaluator.WinnerHand.SecondRank);
            Assert.AreEqual(3, winners.Count);
            CollectionAssert.Contains(winners, secondPlayer, "Second player should be in winners");
            CollectionAssert.Contains(winners, fourthPlayer, "Fourth player should be in winners");
            CollectionAssert.Contains(winners, eighthPlayer, "Eighth player should be in winners");
        }

        //Helper for dealing cards to players
        private void DealCardsToPlayer(Player player, string cards)
        {
            CardList list = MakeCardList(cards);
            foreach (PokerCard card in list)
            {
                player.DrawCard(card);
            }
        }

        //Helper method for adding cards to tests by string
        private CardList MakeCardList(string cards)
        {
            CardList list = new CardList();
            string[] cardStrings = cards.Split(',');
            foreach (string cardString in cardStrings)
            {
                CardRank rank = CardRank.Two;
                CardSuite suite = CardSuite.Clubs;
                switch (cardString[0])
                {
                    case 'A': rank = CardRank.Ace;
                        break;
                    case 'K': rank = CardRank.King;
                        break;
                    case 'Q': rank = CardRank.Queen;
                        break;
                    case 'J': rank = CardRank.Jack;
                        break;
                    case 'T': rank = CardRank.Ten;
                        break;
                    case '9': rank = CardRank.Nine;
                        break;
                    case '8': rank = CardRank.Eight;
                        break;
                    case '7': rank = CardRank.Seven;
                        break;
                    case '6': rank = CardRank.Six;
                        break;
                    case '5': rank = CardRank.Five;
                        break;
                    case '4': rank = CardRank.Four;
                        break;
                    case '3': rank = CardRank.Three;
                        break;
                    case '2': rank = CardRank.Two;
                        break;
                }
                switch (cardString[1])
                {
                    case 'H': suite = CardSuite.Hearts;
                        break;
                    case 'S': suite = CardSuite.Spades;
                        break;
                    case 'D': suite = CardSuite.Diamonds;
                        break;
                    case 'C': suite = CardSuite.Clubs;
                        break;
                }
                list.Add(new PokerCard(rank, suite));
            }
            return list;
        }
    }
}
