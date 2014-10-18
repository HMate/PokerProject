using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProject.PokerGame.AI_Utilities
{
    /// <summary>
    /// Solves the zT(q + Mz)=0  Linear Complementarity Problem with Lemke's algorithm
    /// </summary>
    public class LemkeLCPSolver
    {
        Matrix M;
        Vector q;

        int solutionLength;
        int[] basisSet;
        Matrix table;

        public void SetConstraitMatrix(Matrix m)
        {
            M = m;
        }

        public void SetVector(Vector v)
        {
            q = v;
        }

        /// <summary>
        /// Solves the zT(q + Mz)=0  Linear Complementarity Problem with Lemke's algorithm and gives back z vector
        /// </summary>
        /// <returns></returns>
        public Vector ComputeSolution()
        {
            CheckArguments();
            if (CheckTrivialSolution())
            {
                Vector zTrivial = new Vector(solutionLength);
                return zTrivial;
            }

            table = CreateLemkeTable();
            int tableLength = table.ColumnCount;

            solutionLength = q.Length;
            basisSet = new int[solutionLength];
            for (int i = 0; i < solutionLength; i++)
            {
                basisSet[i] = i;
            }

            //first step
            int z0ColumnIndex = tableLength - 2;
            int qColumnIndex = tableLength - 1;
            int pivotColumn = z0ColumnIndex;
            int pivotRow = GetFirstPivotRow();
            decimal pivotValue = table[pivotRow][pivotColumn];
            int oldBasis = basisSet[pivotRow];
            int cycles = 0;
            Write(cycles);

            // other steps
            while (oldBasis != z0ColumnIndex)
            {
                oldBasis = ChangeBasis(pivotRow, pivotColumn);
                table[pivotRow] = table[pivotRow] * (1 / pivotValue);
                for (int i = 0; i < solutionLength; i++)
                {
                    if (i != pivotRow)
                    {
                        decimal ratio = table[i][pivotColumn];
                        table[i] = table[i] - (ratio * table[pivotRow]);
                    }
                }

                pivotColumn = GetComplementary(oldBasis);
                int index = GetNewPivotRow(pivotColumn, qColumnIndex);
                if (index == -1 && oldBasis != z0ColumnIndex)
                {
                    throw new LCPException("There shouldn't be a second ray");
                }

                pivotRow = index;
                pivotValue = table[pivotRow][pivotColumn];
                cycles++;
                Write(cycles);
            }

            Vector z = new Vector(solutionLength);
            for (int i = 0; i < solutionLength; i++)
            {
                if (basisSet[i] >= solutionLength)
                {
                    z[basisSet[i] - solutionLength] = table[i][qColumnIndex];
                }
            }

            return z;
        }

        private void Write(int cycle)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"TEST/LCPTabla.txt", true))
            {
                file.WriteLine("Cycle: " + cycle.ToString());
                for (int i = 0; i < table.RowCount; i++)
                {
                    file.Write("[ ");
                    for (int j = 0; j < table.ColumnCount; j++)
                    {
                        file.Write(table[i][j].ToString() + "\t,");
                    }
                    file.WriteLine(" ]");
                }
            }
        }

        private void CheckArguments()
        {
            if (M == null || q == null)
            {
                throw new LCPException("M matrix or q vector can't be nulls");
            }
            if (M.ColumnCount != M.RowCount)
            {
                throw new LCPException("Matrix has to have the same row and column count");
            }
        }

        private bool CheckTrivialSolution()
        {
            bool trivial = true;
            for (int i = 0; i < q.Length; i++)
            {
                if (q[i] < 0)
                {
                    trivial = false;
                    break;
                }
            }
            return trivial;
        }

        private Matrix CreateLemkeTable()
        {
            solutionLength = q.Length;
            Matrix W = new Matrix(solutionLength);
            Vector z0 = new Vector(solutionLength);
            for (int i = 0; i < solutionLength; i++)
            {
                z0[i] = -10;
            }
            return W.AppendToRight(-1 * M).AppendToRight(z0).AppendToRight(q);
        }

        private int GetFirstPivotRow()
        {
            int index = 0;
            decimal minValue = q[0];
            for (int i = 1; i < solutionLength; i++)
            {
                if (q[i] < minValue)
                {
                    index = i;
                    minValue = q[i];
                }
            }
            return index;
        }

        /// <summary>
        /// Switches the old basis to the new basis on the index position
        /// </summary>
        /// <param name="oldBasisIndex"></param>
        /// <param name="newBasis"></param>
        /// <returns></returns>
        private int ChangeBasis(int oldBasisIndex, int newBasis)
        {
            int oldBasis = basisSet[oldBasisIndex];
            basisSet[oldBasisIndex] = newBasis;
            return oldBasis;
        }

        private int GetComplementary(int index)
        {
            if (index > solutionLength - 1) return index - solutionLength;
            else return index + solutionLength;
        }

        private int GetNewPivotRow(int pivotColumn, int qColumnIndex)
        {
            int index = -1;
            decimal? minValue = null;
            for (int i = 0; i < solutionLength; i++)
            {
                if (table[i][pivotColumn] > 0)
                {
                    decimal currentQ = table[i][qColumnIndex] / table[i][pivotColumn];
                    if (minValue == null || minValue > currentQ)
                    {
                        index = i;
                        minValue = currentQ;
                    }
                }
            }
            return index;
        }
    }

    public class LCPException : Exception
    {
        public LCPException() : base("Exception occured during solving LCP") { }
        public LCPException(string message) : base(message) { }
        public LCPException(string message, Exception inner) : base(message, inner) { }
        protected LCPException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
