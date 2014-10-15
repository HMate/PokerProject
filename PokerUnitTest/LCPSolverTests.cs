using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerProject.PokerGame.AI_Utilities;

namespace PokerUnitTest
{
    [TestClass]
    public class LCPSolverTests
    {
        [TestMethod]
        public void LemkeTest()
        {
            LemkeLCPSolver solver = new LemkeLCPSolver();
            Matrix M = new Matrix(new decimal [14, 14] {{ 0,  0,  0,  0,  0,  0,  1, -1, -1,  1,  0,  0,  0,  0}
                                                       ,{ 0,  0,  0,  0, -2, -2,  0,  1,  0, -1,  0,  0,  0,  0}
                                                       ,{ 0,  0,  0,  0, -8, -3,  0,  1,  0, -1,  0,  0,  0,  0}
                                                       ,{ 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  1, -1, -1,  1}
                                                       ,{ 0, -6,  0,  0,  0,  0,  0,  0,  0,  0,  0,  1,  0, -1}
                                                       ,{ 0, -6, -9,  0,  0,  0,  0,  0,  0,  0,  0,  1,  0, -1}
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
            solver.SetVector(q);

            Vector z = solver.ComputeSolution();

            Assert.IsTrue(false, String.Format("x=[{0}, {1}, {2}], y=[{3}, {4}, {5}]", z[0], z[1], z[2], z[3], z[4], z[5]));
        }
    }
}
