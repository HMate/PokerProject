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

        [TestInitialize]
        public void SetUp()
        {
            jack = new Player("Jack");
            deck = new CardDeck();
        }

        [TestMethod]
        public void PlayerNamingTest()
        {
            string name = "Jack";

            Assert.AreEqual(name, jack.Name);
        }

        [TestMethod]
        public void PlayerCopyConstructorTest()
        {
           Player jackCopy = new Player(jack);
           CardList jacksCardList = jack.ShowCards();
           CardList jackCopysCardList = jackCopy.ShowCards();

           Assert.AreEqual(jack.Name, jackCopy.Name);
           Assert.AreEqual(jack.ChipCount, jackCopy.ChipCount);
           Assert.AreNotSame(jack, jackCopy);
        }

        [TestMethod]
        public void PlayerHumanCopyTest()
        {
            Player otherPlayer = jack.Clone();

            Assert.AreEqual(jack.Name, otherPlayer.Name);
            Assert.AreNotSame(jack, otherPlayer);
        }

        [TestMethod]
        public void PlayerDrawCardsTest()
        {
            jack.DrawCard(deck);
            jack.DrawCard(deck);

            CardList cards = jack.ShowCards();

            Assert.AreEqual(2, cards.Count);
            Assert.AreNotEqual(cards.ElementAt(0)  , cards.ElementAt(1));
        }

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

        [TestMethod]
        public void PlayerFoldCardsTest()
        {
            jack.DrawCard(deck);
            jack.DrawCard(deck);
            jack.FoldCards();

            CardList cards = jack.ShowCards();

            Assert.AreEqual(0, cards.Count);
        }

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

        [TestMethod]
        public void PlayerPostBlindTest()
        {
            Table table = Table.Instance;
            PlayerPositions rule = table.Rules;
            Pot mainPot = table.MainPot;
            rule.Reset();
            mainPot.Empty();
            rule.SetBigBlindPlayer(jack);

            rule.SetBlind(50);
            jack.ChipCount = 100;
            jack.PostBlind();
            Assert.AreEqual(50, mainPot.Size);
        }

        [TestMethod]
        public void PlayerHaveLessThanBlindTest()
        {
            Table table = Table.Instance;
            PlayerPositions rule = table.Rules;
            Pot mainPot = table.MainPot;
            rule.Reset();
            mainPot.Empty();

            rule.SetBlind(100);
            jack.ChipCount = 80;
            jack.PostBlind();
            Assert.AreEqual(80, mainPot.Size);
        }

        [TestMethod]
        public void PlayerLoseChipsForPostingBlindTest()
        {
            Table table = Table.Instance;
            PlayerPositions rule = table.Rules;
            Pot mainPot = table.MainPot;
            rule.Reset();
            mainPot.Empty();

            rule.SetBlind(150);
            jack.ChipCount = 90;
            jack.PostBlind();
            Assert.AreEqual(90, mainPot.Size);
            jack.PostBlind();
            Assert.AreEqual(90, mainPot.Size,"PostBlind should have done nothing here");
        }

        [TestMethod]
        public void PlayerSetIngameTest()
        {
            jack.SetIngame(true);
            Assert.AreEqual(true, jack.IsIngame());
            jack.SetIngame(false);
            Assert.AreEqual(false, jack.IsIngame());
        }

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
            catch (ArgumentOutOfRangeException e)
            {
                //If player didn't have enough chips then the Pot size shouldn't change.
                Assert.AreEqual(0, mainPot.Size);
                Assert.AreEqual(100, jack.ChipCount);
                throw e;
            }
            
        }
    }
}
