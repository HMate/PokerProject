using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerProject.PokerGame.AI_Utilities;

namespace PokerUnitTest
{
    [TestClass]
    public class LCPSolverTests
    {
        [TestMethod]
        public void LemkeTest1()
        {
            LemkeLCPSolver solver = new LemkeLCPSolver();
            Matrix M = new Matrix(new decimal[4, 4] {{ 0,  0, -1,  -1}
                                                    ,{ 0,  0,  1,  -2}
                                                    ,{ 1, -1,  2,  -2}
                                                    ,{ 1,  2, -2,  4}});
            solver.SetConstraitMatrix(M);
            Vector q = new Vector(2, 2, -2, -6);
            solver.SetVector(q);

            Vector z = solver.ComputeSolution();

            Assert.AreEqual(new Vector(2.8m, 0, 0.8m, 1.2m), z, String.Format("x=[{0}, {1}, {2}, {3}]", z[0], z[1], z[2], z[3]));
        }

        [TestMethod]
        public void LemkeTest2()
        {
            LemkeLCPSolver solver = new LemkeLCPSolver();
            Matrix M = new Matrix(new decimal [14, 14] {{ 0,  0,  0,  9,  9,  9,  1, -1, -1,  1,  0,  0,  0,  0}
                                                       ,{ 0,  0,  0,  7,  9,  9,  0,  1,  0, -1,  0,  0,  0,  0}
                                                       ,{ 0,  0,  0,  9,  1,  6,  0,  1,  0, -1,  0,  0,  0,  0}
                                                       ,{ 9,  3,  9,  0,  0,  0,  0,  0,  0,  0,  1, -1, -1,  1}
                                                       ,{ 9,  9,  9,  0,  0,  0,  0,  0,  0,  0,  0,  1,  0, -1}
                                                       ,{ 9,  9,  0,  0,  0,  0,  0,  0,  0,  0,  0,  1,  0, -1}
                                                       ,{-1,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0}
                                                       ,{ 1, -1, -1,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0}
                                                       ,{ 1,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0}
                                                       ,{-1,  1,  1,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0}
                                                       ,{ 0,  0,  0, -1,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0}
                                                       ,{ 0,  0,  0,  1, -1, -1,  0,  0,  0,  0,  0,  0,  0,  0}
                                                       ,{ 0,  0,  0,  1,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0}
                                                       ,{ 0,  0,  0, -1,  1,  1,  0,  0,  0,  0,  0,  0,  0,  0}});
            solver.SetConstraitMatrix(M);
            Vector q = new Vector(0, 0, 0, 0, 0, 0, 1, 0, -1, 0, 1, 0, -1, 0);
            decimal eps = 0.01M;
            decimal epsVal = eps;
            Vector epsVec = new Vector(14);
            for (int i = 0; i < 14; i++)
            {
                epsVec[i] = epsVal;
                epsVal = epsVal * eps;
            }
            solver.SetVector(q + epsVec);

            Vector z = solver.ComputeSolution();

            Assert.IsTrue(false, String.Format("x=[{0}, {1}, {2}], y=[{3}, {4}, {5}]", z[0], z[1], z[2], z[3], z[4], z[5]));
        }
    }
}
