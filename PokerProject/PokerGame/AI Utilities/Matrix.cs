using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProject.PokerGame.AI_Utilities
{
    public class Vector : IEquatable<Vector>
    {
        private decimal[] components;

        public Vector(params decimal[] numbers)
        {
            components = numbers;
        }

        public Vector(int size)
        {
            components = new decimal[size];
        }

        public int Length
        {
            get { return components.Count(); }
        }

        public decimal this[int index]
        {
            get { return components[index]; }
            set { components[index] = value; }
        }

        public Vector Append(Vector v)
        {
            int mergedCount = Length + v.Length;
            Vector mergedComponents = new Vector(mergedCount);
            for (int i = 0; i < Length; i++)
            {
                mergedComponents[i] = components[i];
            }
            for (int i = 0; i < v.Length; i++)
            {
                mergedComponents[i + Length] = v[i];
            }

            return mergedComponents;
        }

        public Vector Append(decimal d)
        {
            int mergedCount = Length + 1;
            Vector mergedComponents = new Vector(mergedCount);
            for (int i = 0; i < Length; i++)
            {
                mergedComponents[i] = components[i];
            }
            mergedComponents[Length] = d;

            return mergedComponents;
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            if (v1.Length != v2.Length)
            {
                throw new IndexOutOfRangeException("The dimension of the vectors doesn't match!");
            }
            Vector sumVector = new Vector(v1.Length);
            for (int i = 0; i < v1.Length; i++)
            {
                sumVector[i] = RoundToWholeNumber(v1[i] + v2[i]);
            }

            return sumVector;
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            if (v1.Length != v2.Length)
            {
                throw new IndexOutOfRangeException("The dimension of the vectors doesn't match!");
            }
            Vector difVector = new Vector(v1.Length);
            for (int i = 0; i < v1.Length; i++)
            {
                difVector[i] = RoundToWholeNumber(v1[i] - v2[i]);
            }

            return difVector;
        }

        public static Vector operator *(decimal d, Vector v1)
        {
            return v1 * d;
        }

        public static Vector operator *(Vector v1, decimal d)
        {
            Vector mulVector = new Vector(v1.Length);
            for (int i = 0; i < v1.Length; i++)
            {
                mulVector[i] = RoundToWholeNumber(v1[i] * d);
            }

            return mulVector;
        }

        public static decimal operator *(Vector v1, Vector v2)
        {
            if (v1.Length != v2.Length)
            {
                throw new IndexOutOfRangeException("The dimension of the vectors doesn't match!");
            }
            decimal sum = 0;
            for (int i = 0; i < v1.Length; i++)
            {
                sum += (v1[i] * v2[i]);
            }

            return RoundToWholeNumber(sum);
        }

        private static decimal RoundToWholeNumber(decimal d)
        {
            decimal diff = 0.00000000001M;
            if (d - Math.Floor(d) < diff) return Math.Floor(d);
            if (Math.Ceiling(d) - d < diff) return Math.Ceiling(d);
            return d;
        }

        public override bool Equals(object obj)
        {
            Vector v2 = obj as Vector;
            if (v2 == null) return false;
            return this.Equals(v2);
        }

        public bool Equals(Vector v2)
        {
            if (this.Length != v2.Length) return false;
            decimal difference = 0.000000001M;
            for (int i = 0; i < Length; i++)
            {
                if (Math.Abs(this[i] - v2[i]) > difference)
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hash = 0;
            for (int i = 0; i < Length; i++)
            {
                hash = 31 * hash + components[i].GetHashCode();
            }
            return hash;
        }
    }

    public class Matrix : IEquatable<Matrix>
    {
        private Vector[] rows;

        /// <summary>
        /// Creates a rowCount by rowCount sized identity matrix
        /// </summary>
        /// <param name="rowCount"></param>
        public Matrix(int rowCount) : this(rowCount, rowCount) 
        {
            for (int i = 0; i < rowCount; i++)
            {
                rows[i][i] = 1;
            }
        }

        /// <summary>
        /// Creates a rowCount by columnCount size matrix filled with zeros.
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="columnCount"></param>
        public Matrix(int rowCount, int columnCount)
        {
            rows = new Vector[rowCount];
            for (int i = 0; i < rowCount; i++)
            {
                rows[i] = new Vector(columnCount);
            }
        }

        public Matrix(params Vector[] rows)
        {
            if (rows[0] == null)
            {
                throw new ArgumentNullException("Rows can't be null");
            }
            int size = rows[0].Length;
            foreach (var row in rows)
            {
                if (row.Length != size)
                {
                    throw new ArgumentNullException("Rows have to be of same dimensions.");
                }
                if (row == null)
                {
                    throw new ArgumentNullException("Rows can't be null");
                }
            }
            this.rows = rows;
        }

        public Matrix(decimal[,] rows)
        {
            if (rows.Rank != 2)
            {
                throw new ArgumentOutOfRangeException("Only rank 2 arrays can be matrices.");
            }
            int rowNumber = rows.GetLength(0);
            int columnWidth = rows.GetLength(1);
            Vector[] matrix = new Vector[rowNumber];

            for (int rowIndex = 0; rowIndex < rowNumber; rowIndex++)
            {
                decimal[] buf = new decimal[columnWidth];
                for (int j = 0; j < columnWidth; j++)
                {
                    buf[j] = rows[rowIndex, j];
                }
                Vector newRow = new Vector(buf);
                matrix[rowIndex] = newRow;
            }

            this.rows = matrix;
        }

        public int RowCount { get { return rows.Count(); } }

        public int ColumnCount 
        { 
            get 
            {
                if (rows == null || rows[0] == null)
                {
                    return 0;
                }
                return rows[0].Length; 
            } 
        }

        /// <summary>
        /// Returns the row which has this index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector GetRow(int index)
        {
            return rows[index];
        }

        /// <summary>
        /// Sets the row to the vector v which has this index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public void SetRow(int index, Vector v)
        {
            rows[index] = v;
        }

        /// <summary>
        /// Returns the column which has this index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector GetColumn(int index)
        {
            Vector column = new Vector(rows.Count());
            for (int i = 0; i < rows.Count(); i++)
            {
                column[i] = rows[i][index];
            }
            return column;
        }

        /// <summary>
        /// Sets the column to the vector v which has this index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public void SetColumn(int index, Vector v)
        {
            if (v.Length != rows.Count())
            {
                throw new ArgumentOutOfRangeException("Wrong dimension of vector");
            }
            for (int i = 0; i < rows.Count(); i++)
            {
                rows[i][index] = v[i];
            }
        }

        /// <summary>
        /// Returns the row which has this index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector this[int index]
        {
            get { return rows[index]; }
            set { rows[index] = value; }
        }

        /// <summary>
        /// Gives back matrix [this, m]
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public Matrix AppendToRight(Matrix m)
        {
            if (RowCount != m.RowCount)
            {
                throw new ArgumentOutOfRangeException("The row dimensions doesn't match!");
            }
            Matrix mergedMatrix = new Matrix(RowCount, ColumnCount + m.ColumnCount);
            for (int i = 0; i < RowCount; i++)
            {
                mergedMatrix[i] = this[i].Append(m[i]);
            }

            return mergedMatrix;
        }

        public Matrix AppendToRight(Vector v)
        {
            if (RowCount != v.Length)
            {
                throw new ArgumentOutOfRangeException("The row dimensions doesn't match!");
            }
            Matrix mergedMatrix = new Matrix(RowCount, ColumnCount + 1);
            for (int i = 0; i < RowCount; i++)
            {
                mergedMatrix[i] = this[i].Append(v[i]);
            }

            return mergedMatrix;
        }

        /// <summary>
        /// Gives back a matrix [this; m]
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public Matrix AppendToBottom(Matrix m)
        {
            if (ColumnCount != m.ColumnCount)
            {
                throw new ArgumentOutOfRangeException("The column dimensions doesn't match!");
            }
            Matrix mergedMatrix = new Matrix(RowCount + m.RowCount, ColumnCount);
            for (int i = 0; i < RowCount; i++)
            {
                mergedMatrix[i] = this[i];
            }
            for (int i = 0; i < m.RowCount; i++)
            {
                mergedMatrix[i + RowCount] = m[i];
            }

            return mergedMatrix;
        }

        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            if (m1.RowCount != m2.RowCount || m1.ColumnCount != m2.ColumnCount)
            {
                throw new ArgumentOutOfRangeException("The dimension of the matrices doesn't match!");
            }
            Matrix sumMx = new Matrix(m1.RowCount, m1.ColumnCount);
            for (int i = 0; i < m1.RowCount; i++)
            {
                sumMx[i] = m1[i] + m2[i];
            }

            return sumMx;
        }

        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            if (m1.RowCount != m2.RowCount || m1.ColumnCount != m2.ColumnCount)
            {
                throw new ArgumentOutOfRangeException("The dimension of the matrices doesn't match!");
            }
            Matrix diffMx = new Matrix(m1.RowCount, m1.ColumnCount);
            for (int i = 0; i < m1.RowCount; i++)
            {
                diffMx[i] = m1[i] - m2[i];
            }

            return diffMx;
        }

        public static Matrix operator *(Matrix m, decimal d)
        {
            return d * m;
        }

        public static Matrix operator *(decimal d, Matrix m)
        {
            Matrix mulMx = new Matrix(m.RowCount, m.ColumnCount);
            for (int i = 0; i < m.RowCount; i++)
            {
                mulMx[i] = m[i] * d;
            }

            return mulMx;
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            if (m1.ColumnCount != m2.RowCount)
            {
                throw new ArgumentOutOfRangeException("The dimension of the matrices doesn't match!");
            }
            Matrix mulMx = new Matrix(m1.RowCount, m2.ColumnCount);
            for (int i = 0; i < m1.RowCount; i++)
            {
                for (int j = 0; j < m2.ColumnCount; j++)
                {
                    mulMx[i][j] = m1.GetRow(i) * m2.GetColumn(j);
                }
            }

            return mulMx;
        }

        public override bool Equals(object obj)
        {
            Matrix m2 = obj as Matrix;
            if (m2 == null) return false;
            return this.Equals(m2);
        }

        public bool Equals(Matrix m2)
        {
            if (this.rows.Count() != m2.rows.Count()) return false;
            for (int i = 0; i < this.rows.Count(); i++)
            {
                if (!this.rows[i].Equals(m2.rows[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hash = 0;
            for (int i = 0; i < RowCount; i++)
            {
                hash = 31 * hash + rows[i].GetHashCode();
            }
            return hash;
        }
    }
}
