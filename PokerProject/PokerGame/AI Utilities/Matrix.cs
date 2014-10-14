using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProject.PokerGame.AI_Utilities
{
    public class Vector<T> : IEquatable<Vector<T>>
    {
        private T[] components;

        public Vector(params T[] numbers)
        {
            components = numbers;
        }

        public Vector(int size)
        {
            components = new T[size];
        }

        public int Count
        {
            get { return components.Count(); }
        }

        public T this[int index]
        {
            get { return components[index]; }
            set { components[index] = value; }
        }

        public static Vector<T> operator +(Vector<T> v1, Vector<T> v2)
        {
            if (v1.Count != v2.Count)
            {
                throw new IndexOutOfRangeException("The dimension of the vectors doesn't match!");
            }
            Vector<T> sumVector = new Vector<T>(v1.Count);
            for (int i = 0; i < v1.Count; i++)
            {
                sumVector[i] = (dynamic)v1[i] + v2[i];
            }

            return sumVector;
        }

        public static Vector<T> operator *(double d, Vector<T> v1)
        {
            return v1 * d;
        }

        public static Vector<T> operator *(Vector<T> v1, double d)
        {
            Vector<T> mulVector = new Vector<T>(v1.Count);
            for (int i = 0; i < v1.Count; i++)
            {
                mulVector[i] = (T)((dynamic)v1[i] * d);
            }

            return mulVector;
        }

        public static double operator *(Vector<T> v1, Vector<T> v2)
        {
            if (v1.Count != v2.Count)
            {
                throw new IndexOutOfRangeException("The dimension of the vectors doesn't match!");
            }
            double sum = 0;
            for (int i = 0; i < v1.Count; i++)
            {
                sum += (double)((dynamic)v1[i] * v2[i]);
            }

            return sum;
        }

        public static Vector<T> operator -(Vector<T> v1, Vector<T> v2)
        {
            return v1 + (-1 * v2);
        }

        public override bool Equals(object obj)
        {
            Vector<T> v2 = obj as Vector<T>;
            if (v2 == null) return false;
            return this.Equals(v2);
        }

        public bool Equals(Vector<T> v2)
        {
            if (this.Count != v2.Count) return false;
            for (int i = 0; i < Count; i++)
            {
                if (!this[i].Equals(v2[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }

    public class Matrix<T> 
    {
        private Vector<T>[] rows;

        public Matrix(params Vector<T>[] rows)
        {
            this.rows = rows;
        }

        public Matrix(T[,] rows)
        {
            if (rows.Rank != 2)
            {
                throw new ArgumentOutOfRangeException("Only rank 2 arrays can be matrices.");
            }
            Vector<T>[] matrix = new Vector<T>[rows.GetLength(1)];
            int rowIndex = 0;
            foreach (var row in rows)
            {
                Vector<T> newRow = new Vector<T>(row);
                matrix[rowIndex] = newRow;
                rowIndex++;
            }
        }
    }
}
