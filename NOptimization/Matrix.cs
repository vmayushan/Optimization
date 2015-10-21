using System;

namespace NOptimization
{
    public struct Matrix
    {
        private int nRows;
        private int nCols;
        private double[,] matrix;
        public Matrix(int nRows, int nCols)
        {
            this.nRows = nRows;
            this.nCols = nCols;
            this.matrix = new double[nRows, nCols];
            for (int i = 0; i < nRows; i++)
            {
                for (int j = 0; j < nCols; j++)
                {
                    matrix[i, j] = 0.0;
                }
            }
        }
        public Matrix(double[,] matrix)
        {
            this.nRows = matrix.GetLength(0);
            this.nCols = matrix.GetLength(1);
            this.matrix = matrix;
        }
        public Matrix IdentityMatrix()
        {
            Matrix m = new Matrix(nRows, nCols);
            for (int i = 0; i < nRows; i++)
            {
                for (int j = 0; j < nCols; j++)
                {
                    if (i == j)
                    {
                        m[i, j] = 1;
                    }
                }
            }
            return m;
        }
        public double this[int m, int n]
        {
            get
            {
                if (m < 0 || m > nRows)
                {
                    throw new Exception("m-неправильный индекс!");
                }
                if (n < 0 || n > nCols)
                {
                    throw new Exception("n-неправильный индекс!");
                }
                return matrix[m, n];
            }
            set { matrix[m, n] = value; }
        }
        public int GetnRows
        {
            get { return nRows; }
        }
        public int GetnCols
        {
            get { return nCols; }
        }
        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            if (!Matrix.CompareDimension(m1, m2))
            {
                throw new Exception("Не совпадают размерности");
            }
            Matrix result = new Matrix(m1.GetnRows, m1.GetnCols);
            for (int i = 0; i < m1.GetnRows; i++)
            {
                for (int j = 0; j < m1.GetnCols; j++)
                {
                    result[i, j] = m1[i, j] + m2[i, j];
                }
            }
            return result;
        }
        public static Matrix operator -(Matrix m)
        {
            for (int i = 0; i < m.GetnRows; i++)
            {
                for (int j = 0; j < m.GetnCols; j++)
                {
                    m[i, j] = -m[i, j];
                }
            }
            return m;
        }
        public static Matrix operator /(Matrix m, double d)
        {
            Matrix result = new Matrix(m.GetnRows, m.GetnCols);
            for (int i = 0; i < m.GetnRows; i++)
            {
                for (int j = 0; j < m.GetnCols; j++)
                {
                    result[i, j] = m[i, j] / d;
                }
            }
            return result;
        }
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            if (m1.GetnCols != m2.GetnRows)
            {
                throw new Exception("Не совпадает размерность!");
            }
            Matrix result = new Matrix(m1.GetnRows, m2.GetnCols);
            for (int i = 0; i < m1.GetnRows; ++i)
            {
                for (int j = 0; j < m2.GetnCols; ++j)
                {
                    result[i, j] = 0;
                    for (int q = 0; q < m1.GetnCols; ++q)
                        result[i, j] += m1[i, q] * m2[q, j];
                }
            }
            return result;
        }
        public Matrix GetTranspose()
        {
            Matrix m = this;
            m.Transpose();
            return m;
        }
        public void Transpose()
        {
            Matrix m = new Matrix(nCols, nRows);
            for (int i = 0; i < nRows; i++)
            {
                for (int j = 0; j < nCols; j++)
                {
                    m[j, i] = matrix[i, j];
                }
            }
            this = m;
        }
        public static bool CompareDimension(Matrix m1, Matrix m2)
        {
            if (m1.GetnRows == m2.GetnRows && m1.GetnCols == m2.GetnCols)
                return true;
            else
                return false;
        }
        public Matrix ReplaceCol(Vector vec, int n)
        {
            if (n < 0 || n > nCols)
            {
                throw new Exception("Неправильный индекс столбца");
            }
            if (vec.GetVectorSize != nRows)
            {
                throw new Exception("Размерности не совпадают");
            }
            for (int i = 0; i < nRows; i++)
            {
                matrix[i, n] = vec[i];
            }
            return new Matrix(matrix);
        }
        public static implicit operator Vector(Matrix m)
        {
            Vector ColVector = new Vector(m.nRows);
            for (int i = 0; i < m.nRows; i++)
            {
                ColVector[i] = m[i, 0];
            }
            return ColVector;
        }
    }
}
