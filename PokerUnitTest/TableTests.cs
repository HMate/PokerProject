using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerProject.PokerGame;

namespace PokerUnitTest
{
    [TestClass]
    public class TableTests
    {
        Table table = Table.Instance;

        [TestMethod]
        public void RuleSetBlindTest()
        {
            for (int i = 0; i < 500; i += 50)
            {
                table.SetBlind(i);
                Assert.AreEqual(i, table.GetBlind());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RuleSetInvalidBlindTest()
        {
            table.SetBlind(-20);
        }

        [TestMethod]
        public void RulesResetTest()
        {
            table.SetBlind(50);
            table.Reset();
            Assert.AreEqual(0, table.GetBlind());
        }
    }
}
