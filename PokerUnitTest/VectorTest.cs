using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerProject.PokerGame.AI_Utilities;

namespace PokerUnitTest
{
    [TestClass]
    public class VectorTest
    {
        #region Vectors
        [TestMethod]
        public void VectorInitTest()
        {
            int[] a = {1, 2, 3, 4, 5};
            Vector<int> v = new Vector<int>(a);

            Assert.AreEqual(5, v.Count);
        }

        [TestMethod]
        public void VectorEqualityTest()
        {
            int[] a = { 1, 2, 3, 4, 5 };
            Vector<int> v1 = new Vector<int>(a);
            Vector<int> v2 = new Vector<int>(1, 2, 3, 4, 5);
            Assert.IsTrue(v1.Equals(v2));
            Assert.AreEqual(v1, v2);

            double[] a2 = { 1.1D, 2.2D, 3.3D, 4.4D, 5.5D };
            Vector<double> vd1 = new Vector<double>(a2);
            double[] b2 = { 1.1D, 2.2D, 3.3D, 4.4D, 5.5D };
            Vector<double> vd2 = new Vector<double>(b2);
            Assert.IsTrue(vd1.Equals(vd2));
        }

        [TestMethod]
        public void VectorNonEqualityTest()
        {
            int[] a = { 1, 2, 3, 4, 5 };
            Vector<int> v1 = new Vector<int>(a);
            int[] b = { 1, 2, 3, 4, 10 };
            Vector<int> v2 = new Vector<int>(b);
            double[] a2 = { 1D, 2D, 3D, 4D, 5D };
            Vector<double> vd1 = new Vector<double>(a2);

            Assert.IsFalse(v1.Equals(v2));
            Assert.IsFalse(v1.Equals(vd1));
        }

        [TestMethod]
        public void VectorIndexTest()
        {
            Vector<int> v1 = new Vector<int>(1, 2, 3, 4, 5);
            v1[2] = -30;
            Vector<int> v2 = new Vector<int>(1, 2, -30, 4, 5);

            Assert.AreEqual(v1, v2);
        }

        [TestMethod]
        public void VectorSumTest()
        {
            int[] a = { 1, 2, 3, 4, 5 };
            Vector<int> v1 = new Vector<int>(a);
            int[] b = { 1, 2, 3, 4, 10 };
            Vector<int> v2 = new Vector<int>(b);

            Vector<int> vd1 = v1 + v2;

            Vector<int> expected = new Vector<int>(2, 4, 6, 8, 15);
            Assert.AreEqual(expected, vd1);
        }

        [TestMethod]
        public void VectorSubtractTest()
        {
            int[] a = { 1, 2, 3, 4, 5 };
            Vector<int> v1 = new Vector<int>(a);
            int[] b = { 1, 2, 3, 4, 10 };
            Vector<int> v2 = new Vector<int>(b);

            Vector<int> vd1 = v1 - v2;

            Vector<int> expected = new Vector<int>(0, 0, 0, 0, -5);
            Assert.AreEqual(expected, vd1);
        }

        [TestMethod]
        public void VectorScalarMulTest()
        {
            int[] a = { 1, 2, 3, 4, 5 };
            Vector<int> v1 = new Vector<int>(a);

            Vector<int> vd1 = 2 * v1;
            Vector<int> expected = new Vector<int>(2, 4, 6, 8, 10);
            Assert.AreEqual(expected, vd1);

            Vector<int> vd2 = 2.2D * v1;
            Vector<int> expected2 = new Vector<int>(2, 4, 6, 8, 11);
            Assert.AreEqual(expected2, vd2);
        }

        [TestMethod]
        public void VectorVectorMulTest()
        {
            Vector<int> v1 = new Vector<int>(1, 2, 3, 4, 5);
            Vector<int> v2 = new Vector<int>(2, 1, 4, 7, 100);

            double vd1 = v2 * v1;
            double expected = 2 + 2 + 12 + 28 + 500;
            Assert.AreEqual(expected, vd1);
        }

#endregion

        #region Matrices

        [TestMethod]
        public void MatrixInitTest()
        {
            int[,] a = new int[2,5] { {1, 2, 3, 4, 5}, {10, 20, 30, 40, 50} };

            Assert.AreEqual(5, a[0, 4]);
            Assert.AreEqual(2, a.Rank);
            Assert.AreEqual(5, a.GetLength(1));
        }



        #endregion
    }
}
