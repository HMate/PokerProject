using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerProject.PokerGame;
using PokerProject.PokerGame.CardClasses;
using PokerProject.PokerGame.PlayerClasses;
using System.Collections.Generic;
using System.Linq;

namespace PokerUnitTest
{

    [TestClass]
    public class GameTests
    {
        Game game;

        [TestInitialize]
        public void SetUp()
        {
            game = new Game();
        }


    }
}
