using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerProject.PokerGame;

namespace PokerUnitTest
{
    [TestClass]
    public class TableTests
    {
        Table table = Table.Instance;

        /*
         * Test for setting and getting the big blind from the table.
         * */
        [TestMethod]
        public void TableSetBlindTest()
        {
            for (int i = 0; i < 500; i += 50)
            {
                table.SetBlind(i);
                Assert.AreEqual(i, table.GetBigBlind());
            }
        }

        /*
         * Test for getting the small blind from the table.
         * */
        [TestMethod]
        public void TableSmallBlindTest()
        {
            for (int i = 0; i < 500; i += 50)
            {
                table.SetBlind(i);
                Assert.AreEqual(i/2, table.GetSmallBlind());
            }
        }

        /*
         * Test that the blind cannot be negative.
         * If the blind is set to negative, the table throws an exception.
         * */
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TableSetInvalidBlindTest()
        {
            table.SetBlind(-20);
        }

        /*
         * Test for resetting the blind to 0.
         * */
        [TestMethod]
        public void TableResetBlindTest()
        {
            table.SetBlind(50);
            table.Reset();
            Assert.AreEqual(0, table.GetBigBlind());
        }
    }
}
