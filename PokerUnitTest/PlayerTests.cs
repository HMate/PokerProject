using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using PokerProject.PokerGame;
using PokerProject.PokerGame.CardClasses;
using PokerProject.PokerGame.PlayerClasses;

namespace PokerUnitTest
{
    [TestClass]
    public class PlayerTests
    {
        Player jack;
        CardDeck deck;

        /*
         * Initializes before every test.
         * */
        [TestInitialize]
        public void SetUp()
        {
            jack = new Player("Jack");
            deck = new CardDeck();
        }

        /*
         * Test if the Players name matches what he was given.
         * */
        [TestMethod]
        public void PlayerNamingTest()
        {
            string name = "Jack";

            Assert.AreEqual(name, jack.Name);
        }

        /*
         * Test for copying a player with constructor.
         * A copy of a palyer should have the same name and chips, and have the same cards.
         * */
        [TestMethod]
        public void PlayerCopyConstructorTest()
        {
            jack.DrawCard(deck.DealOneCard());
            Player jackCopy = new Player(jack);

            AssertTwoPlayersAreEqual(jack, jackCopy);
        }

        /*
         * Test for copying a player with Clone method
         * */
        [TestMethod]
        public void PlayerCopyWithCloneMethodTest()
        {
            jack.DrawCard(deck.DealOneCard());
            Player jackCopy = jack.Clone();

            AssertTwoPlayersAreEqual(jack, jackCopy);
        }

        private void AssertTwoPlayersAreEqual(Player player, Player playerCopy)
        {
            Assert.AreEqual(player.Name, playerCopy.Name);
            Assert.AreEqual(player.ChipCount, playerCopy.ChipCount);
            Assert.AreEqual(player.ShowCards(), playerCopy.ShowCards());
            Assert.AreNotSame(player, playerCopy);
        }

        /*
         * Tests the players draw card method in general.
         * */
        [TestMethod]
        public void PlayerDrawCardsTest()
        {
            jack.DrawCard(deck);
            jack.DrawCard(deck);

            CardList cards = jack.ShowCards();

            Assert.AreEqual(2, cards.Count);
            Assert.AreNotEqual(cards.ElementAt(0)  , cards.ElementAt(1));
        }

        /*
         * Test for checking players get those cards what they've been dealt.
         * */
        [TestMethod]
        public void PlayerDrawPredefinedCardsTest()
        {
            PokerCard card1 = new PokerCard(CardRank.Five, CardSuite.Hearts);
            PokerCard card2 = new PokerCard(CardRank.Ten, CardSuite.Clubs);
            jack.DrawCard(card1);
            jack.DrawCard(card2);
            CardList cardList = jack.ShowCards();

            Assert.AreEqual(cardList.ElementAt(0), card1);
            Assert.AreEqual(cardList.ElementAt(1), card2);
        }

        /*
         * Tests that players get different cards from a deck.
         * */
        [TestMethod]
        public void PlayersGetDifferentCardsTest()
        {
            Player ben = new Player();
            jack.DrawCard(deck);
            jack.DrawCard(deck);
            ben.DrawCard(deck);
            ben.DrawCard(deck);

            CardList bensCards = ben.ShowCards();
            CardList jacksCards = jack.ShowCards();

            Assert.AreNotEqual(bensCards.ElementAt(0), jacksCards.ElementAt(0));
            Assert.AreNotEqual(bensCards.ElementAt(0), jacksCards.ElementAt(1));
            Assert.AreNotEqual(bensCards.ElementAt(1), jacksCards.ElementAt(0));
            Assert.AreNotEqual(bensCards.ElementAt(1), jacksCards.ElementAt(1));
        }

        /*
         * Tests that a player folds his cards.
         * */
        [TestMethod]
        public void PlayerFoldCardsTest()
        {
            jack.DrawCard(deck);
            jack.DrawCard(deck);
            jack.FoldCards();

            CardList cards = jack.ShowCards();

            Assert.AreEqual(0, cards.Count);
        }

        /*
         * Test if a player has the right amount of chips, after increasing it.
         * */
        [TestMethod]
        public void PlayerChipCountTest()
        {
            jack.ChipCount = 1000;
            jack.IncreaseChipCount(500);
            Assert.AreEqual(1500, jack.ChipCount);
        }

        [TestMethod]
        public void PlayerHasNoChipsTest()
        {
            Assert.AreEqual(0, jack.ChipCount);
        }

        [TestMethod]
        public void PlayerTakeAwayChipsTest()
        {
            jack.IncreaseChipCount(1000);
            jack.DecreaseChipCount(300);
            Assert.AreEqual(700, jack.ChipCount);
        }

        /*
         * Player throws an exception if you try to take away more chips from him than the amount he has.
         * */
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PlayerTakeAwayTooMuchChipsTest()
        {
            jack.IncreaseChipCount(500);
            jack.DecreaseChipCount(700);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PlayerChipCountGetsNegativeChipsTest()
        {
            jack.ChipCount = -400;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PlayerGetsNegativeChipsTest()
        {
            jack.IncreaseChipCount(-400);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PlayerTakeAwayNegativeChipsTest()
        {
            jack.DecreaseChipCount(-400);
        }

        /*
         * Tests if the player can post a blind.
         * */
        [TestMethod]
        public void PlayerPostBigBlindTest()
        {
            Table table = Table.Instance;
            SetTableForTest();
            Player testPlayer = GetPlayerAtPosition("Big Blind");

            table.SetBlind(50);
            testPlayer.ChipCount = 100;
            testPlayer.PostBlind();
            Assert.AreEqual(50, table.MainPot.Size);
        }

        /*
         * Tests if palyer posts small blind if he is the small blind.
         * */
        [TestMethod]
        public void PlayerPostSmallBlindTest()
        {
            Table table = Table.Instance;
            SetTableForTest();
            Player testPlayer = GetPlayerAtPosition("Small Blind");

            table.SetBlind(200);
            testPlayer.ChipCount = 1000;
            testPlayer.PostBlind();
            Assert.AreEqual(100, table.MainPot.Size);
        }

        /*
         * If player have less chips than the blind, he posts all of his chips.
         * */
        [TestMethod]
        public void PlayerHaveLessThanBlindTest()
        {
            Table table = Table.Instance;
            SetTableForTest();
            Player testPlayer = GetPlayerAtPosition("Big Blind");

            table.SetBlind(100);
            testPlayer.ChipCount = 80;
            testPlayer.PostBlind();
            Assert.AreEqual(80, table.MainPot.Size);
        }

        /*
         * A player should lose chips when he posts a blind.
         * */
        [TestMethod]
        public void PlayerLoseChipsForPostingBlindTest()
        {
            Table table = Table.Instance;
            SetTableForTest();
            Player testPlayer = GetPlayerAtPosition("Big Blind");

            table.SetBlind(150);
            jack.ChipCount = 90;
            jack.PostBlind();
            Assert.AreEqual(90, table.MainPot.Size);
            jack.PostBlind();
            Assert.AreEqual(90, table.MainPot.Size, "PostBlind should have done nothing here");
        }

        /*
         * Resets the table for other test methods, and puts the default test player among the players.
         * */
        private void SetTableForTest()
        {
            Table table = Table.Instance;
            PlayerPositions positions = table.Positions;
            PlayerQueue queue = table.Players;
            Pot mainPot = table.MainPot;
            positions.ResetPositions();
            mainPot.Empty();
            queue.Clear();
            queue.AddPlayer(jack);
        }

        private Player GetPlayerAtPosition(String blindPosition)
        {
            Table table = Table.Instance;

            Player testPlayer = table.Players.GetNextPlayer();
            if (blindPosition.Equals("Big Blind"))
            {
                table.Positions.SetBigBlind(testPlayer);
            }
            else
            {
                table.Positions.SetSmallBlind(testPlayer);
            }
            return testPlayer;
        }

        [TestMethod]
        public void PlayerSetIngameTest()
        {
            jack.SetIngame(true);
            Assert.AreEqual(true, jack.IsIngame());
            jack.SetIngame(false);
            Assert.AreEqual(false, jack.IsIngame());
        }

        /*
         * Testing player's Bet() method
         * */
        [TestMethod]
        public void PlayerBetTest()
        {
            Table table = Table.Instance;
            Pot mainPot = table.MainPot;

            int startingChipCount = 100;
            for (int betAmount = 0; betAmount < startingChipCount; betAmount += 10)
            {
                mainPot.Empty();
                jack.ChipCount = startingChipCount;
                jack.Bet(betAmount);
                Assert.AreEqual(betAmount, mainPot.Size);
                Assert.AreEqual(startingChipCount - betAmount, jack.ChipCount);
            }
        }

        /*
         * Testing if a player bets too much.
         * Should throw an exception.
         * */
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PlayerBetTooMuchTest()
        {
            Table table = Table.Instance;
            Pot mainPot = table.MainPot;
            mainPot.Empty();

            int startingChipCount = 100;
            int betAmount = 150;
            jack.ChipCount = startingChipCount;
            try
            {
                jack.Bet(betAmount);
            }
            catch (ArgumentOutOfRangeException)
            {
                //If player didn't have enough chips then the Pot size shouldn't change.
                Assert.AreEqual(0, mainPot.Size);
                Assert.AreEqual(100, jack.ChipCount);
                //throw again as we shouldn't reach the part after the catch block
                throw;
            }
            
        }
    }
}
