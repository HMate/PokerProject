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
            decimal[] a = { 1, 2, 3, 4, 5 };
            Vector v = new Vector(a);

            Assert.AreEqual(5, v.Length);
        }

        [TestMethod]
        public void VectorEqualityTest()
        {
            decimal[] a = { 1, 2, 3, 4, 5 };
            Vector v1 = new Vector(a);
            Vector v2 = new Vector(1, 2, 3, 4, 5);
            Assert.IsTrue(v1.Equals(v2));
            Assert.AreEqual(v1, v2);

            decimal[] a2 = { 1.1M, 2.2M, 3.3M, 4.4M, 5.5M };
            Vector vd1 = new Vector(a2);
            decimal[] b2 = { 1.1M, 2.2M, 3.3M, 4.4M, 5.5M };
            Vector vd2 = new Vector(b2);
            Assert.IsTrue(vd1.Equals(vd2));
        }

        [TestMethod]
        public void VectorNonEqualityTest()
        {
            decimal[] a = { 1, 2, 3, 4, 5 };
            Vector v1 = new Vector(a);
            decimal[] b = { 1, 2, 3, 4, 10 };
            Vector v2 = new Vector(b);

            Assert.IsFalse(v1.Equals(v2));
        }

        [TestMethod]
        public void VectorIndexTest()
        {
            Vector v1 = new Vector(1, 2, 3, 4, 5);
            v1[2] = -30;
            Vector v2 = new Vector(1, 2, -30, 4, 5);

            Assert.AreEqual(v1, v2);
        }

        [TestMethod]
        public void VectorSumTest()
        {
            decimal[] a = { 1, 2, 3, 4, 5 };
            Vector v1 = new Vector(a);
            decimal[] b = { 1, 2, 3, 4, 10 };
            Vector v2 = new Vector(b);

            Vector vd1 = v1 + v2;

            Vector expected = new Vector(2, 4, 6, 8, 15);
            Assert.AreEqual(expected, vd1);
        }

        [TestMethod]
        public void VectorSubtractTest()
        {
            decimal[] a = { 1, 2, 3, 4, 5 };
            Vector v1 = new Vector(a);
            decimal[] b = { 1, 2, 3, 4, 10 };
            Vector v2 = new Vector(b);

            Vector vd1 = v1 - v2;

            Vector expected = new Vector(0, 0, 0, 0, -5);
            Assert.AreEqual(expected, vd1);
        }

        [TestMethod]
        public void VectorScalarMulTest()
        {
            decimal[] a = { 1, 2, 3, 4, 5 };
            Vector v1 = new Vector(a);
            
            Vector vd = 2.2m * v1;
            Vector expected = new Vector(2.2m, 4.4m, 6.6m, 8.8m, 11m);
            Assert.AreEqual(expected, vd, String.Format("actual: {0} , {1} , {2} , {3} , {4} ; expected: {5} , {6} , {7} , {8} , {9}",
                vd[0], vd[1], vd[2], vd[3], vd[4], expected[0], expected[1], expected[2], expected[3], expected[4]));
        }

        [TestMethod]
        public void VectorVectorMulTest()
        {
            Vector v1 = new Vector(1, 2, 3, 4, 5);
            Vector v2 = new Vector(2, 1, 4, 7, 100);

            decimal vd1 = v2 * v1;
            decimal expected = 2 + 2 + 12 + 28 + 500;
            Assert.AreEqual(expected, vd1);
        }

        [TestMethod]
        public void VectorAppendTest()
        {
            Vector v1 = new Vector(1, 2, 3, 4, 5);
            Vector v2 = new Vector(2, 1, 4, 7, 100);

            Vector v = v1.Append(v2);
            Vector expected = new Vector(1, 2, 3, 4, 5, 2, 1, 4, 7, 100);
            Assert.AreEqual(expected, v);
        }

#endregion

        #region Matrices

        [TestMethod]
        public void MatrixZeroInitTest()
        {
            Matrix m0 = new Matrix(2, 5);
            Matrix expected = new Matrix(new decimal[2, 5] { { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } });

            Assert.AreEqual(expected, m0);
        }

        [TestMethod]
        public void MatrixIdentityInitTest()
        {
            Matrix m0 = new Matrix(3);
            Matrix expected = new Matrix(new decimal[3, 3] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } });

            Assert.AreEqual(expected, m0);
        }

        [TestMethod]
        public void MatrixInitTest()
        {
            decimal[,] a = new decimal[2,5] { {1, 2, 3, 4, 5}, {10, 20, 30, 40, 50} };

            Matrix m1 = new Matrix(a);
            Vector v = new Vector(1, 2, 3, 4, 5);
            Matrix m2 = new Matrix(v, new Vector(10, 20, 30, 40, 50));

            Assert.AreEqual(m1, m2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MatrixNullTest()
        {
            Matrix m2 = new Matrix(null, null);
        }

        [TestMethod]
        public void MatrixEqualityTest()
        {
            decimal[,] a = new decimal[2, 5] { { 1, 2, 3, 4, 5 }, { 10, 20, 30, 40, 50 } };
            Matrix m1 = new Matrix(a);

            m1[1][3] = 45;

            Vector[] v = new Vector[2] { new Vector(1, 2, 3, 4, 5), new Vector(10, 20, 30, 45, 50) };
            Matrix m2 = new Matrix(v);

            Assert.AreEqual(m1, m2);
        }

        [TestMethod]
        public void MatrixRowCountTest()
        {
            Matrix m1 = new Matrix(new decimal[2, 5] { { 1, 2, 3, 4, 5 }, { 10, 20, 30, 40, 50 } });
            Assert.AreEqual(2, m1.RowCount);
        }

        [TestMethod]
        public void MatrixColumnCountTest()
        {
            Matrix m1 = new Matrix(new decimal[2, 5] { { 1, 2, 3, 4, 5 }, { 10, 20, 30, 40, 50 } });
            Assert.AreEqual(5, m1.ColumnCount);
        }

        [TestMethod]
        public void MatrixGetColumnTest()
        {
            Matrix m1 = new Matrix(new decimal[3, 4] { { 1, 2, 3, 4 }, { 10, 20, 30, 40 } , {100, 200, 300, 400}});
            Vector v = m1.GetColumn(1);

            Vector expected = new Vector(2, 20, 200);
            Assert.AreEqual(expected, v);
        }

        [TestMethod]
        public void MatrixSetColumnTest()
        {
            Matrix m1 = new Matrix(new decimal[3, 4] { { 1, 2, 3, 4 }, { 10, 20, 30, 40 }, { 100, 200, 300, 400 } });
            m1.SetColumn(3, new Vector(-33, -444, -5555));

            Matrix expected = new Matrix(new decimal[3, 4] { { 1, 2, 3, -33 }, { 10, 20, 30, -444 }, { 100, 200, 300, -5555 } });
            Assert.AreEqual(expected, m1);
        }

        [TestMethod]
        public void MatrixAppendRightTest()
        {
            Matrix m1 = new Matrix(new decimal[3, 2] { { 1, 2 }, { 10, 20 }, { 100, 200 } });
            Matrix m2 = new Matrix(new decimal[3, 2] { { 3, 4 }, { 30, 40 }, { 300, 400 } });
            Matrix appendedMx = m1.AppendToRight(m2);

            Matrix expected = new Matrix(new decimal[3, 4] { { 1, 2, 3, 4 }, { 10, 20, 30, 40 }, { 100, 200, 300, 400 } });
            Assert.AreEqual(expected, appendedMx);
        }

        [TestMethod]
        public void MatrixAppendBottomTest()
        {
            Matrix m1 = new Matrix(new decimal[3, 2] { { 1, 2 }, { 10, 20 }, { 100, 200 } });
            Matrix m2 = new Matrix(new decimal[3, 2] { { 3, 4 }, { 30, 40 }, { 300, 400 } });
            Matrix appendedMx = m1.AppendToBottom(m2);

            Matrix expected = new Matrix(new decimal[6, 2] { { 1, 2 }, { 10, 20 }, { 100, 200 }, { 3, 4 }, { 30, 40 }, { 300, 400 } });
            Assert.AreEqual(expected, appendedMx);
        }

        [TestMethod]
        public void MatrixSumTest()
        {
            Matrix m1 = new Matrix(new decimal[3, 2] { { 1, 2 }, { 10, 20 }, { 100, 200 } });
            Matrix m2 = new Matrix(new decimal[3, 2] { { 3, 4 }, { 30, 40 }, { 300, 400 } });
            Matrix appendedMx = m1 + m2;

            Matrix expected = new Matrix(new decimal[3, 2] { { 4, 6 }, { 40, 60 }, { 400, 600 } });
            Assert.AreEqual(expected, appendedMx);
        }

        [TestMethod]
        public void MatrixSubstractTest()
        {
            Matrix m1 = new Matrix(new decimal[3, 2] { { 1, 2 }, { 10, 20 }, { 100, 200 } });
            Matrix m2 = new Matrix(new decimal[3, 2] { { 3, 4 }, { 30, 40 }, { 300, 400 } });
            Matrix appendedMx = m1 - m2;

            Matrix expected = new Matrix(new decimal[3, 2] { { -2, -2 }, { -20, -20 }, { -200, -200 } });
            Assert.AreEqual(expected, appendedMx);
        }

        [TestMethod]
        public void MatrixMultiplyByScalarTest()
        {
            Matrix m1 = new Matrix(new decimal[3, 2] { { 1, 2 }, { 10, 20 }, { 100, 200 } });
            Matrix mulMx = m1 * 11;

            Matrix expected = new Matrix(new decimal[3, 2] { { 11, 22 }, { 110, 220 }, { 1100, 2200 } });
            Assert.AreEqual(expected, mulMx);
        }

        [TestMethod]
        public void MatrixMultiplyByMxTest()
        {
            Matrix m1 = new Matrix(new decimal[3, 2] { { 1, 2 }, { 10, 20 }, { 100, 200 } });
            Matrix m2 = new Matrix(new decimal[2, 3] { { 3, 4, 5 }, { 30, 40, 50 } });
            Matrix mulMx = m1 * m2;

            Matrix expected = new Matrix(new decimal[3, 3] { { 63, 84, 105 }, { 630, 840, 1050 }, { 6300, 8400, 10500 } });
            Assert.AreEqual(expected, mulMx);
        }

        #endregion
    }
}
