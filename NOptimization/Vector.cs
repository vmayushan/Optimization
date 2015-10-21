using System;

namespace NOptimization
{
    public struct Vector
    {
        private int ndim;
        private double[] vector;
        public Vector(int ndim)
        {
            this.ndim = ndim;
            this.vector = new double[ndim];
            for (int i = 0; i < ndim; i++)
            {
                vector[i] = 0.0;
            }
        }
        public Vector(double[] vector)
        {
            this.ndim = vector.Length;
            this.vector = vector;
        }
        public Vector(double x, double y)
        {
            this.ndim = 2;
            this.vector = new double[] {x,y};
        }
        public Vector(double x, double y, double z)
        {
            this.ndim = 3;
            this.vector = new double[] { x, y, z };
        }
        public double this[int i]
        {
            get
            {
                if (i < 0 || i > ndim)
                {
                    throw new Exception("Requested vector index is out of range!");
                }
                return vector[i];
            }
            set { vector[i] = value; }
        }
        public double LengthSquared
        {
            get { return this.GetNormSquare(); }
        }
        public double X
        {
            get { return vector[0]; }
            set { vector[0] = value;  }
        }
        public double Y
        {
            get { return vector[1]; }
            set { vector[1] = value; }
        }
        public double Z
        {
            get { return vector[2]; }
            set { vector[2] = value; }
        }
        public int GetVectorSize
        {
            get { return ndim; }
        }
        public override string ToString()
        {
            if (ndim == 0)
                return "null";
            string str = "(";
            for (int i = 0; i < ndim - 1; i++)
            {
                str += vector[i].ToString() + ", ";
            }
            str += vector[ndim - 1].ToString() + ")";
            return str;
        }
        public static Vector operator +(Vector v)
        {
            return v;
        }
        public static Vector operator +(Vector v1, Vector v2)
        {
            Vector result = new Vector(v1.ndim);
            for (int i = 0; i < v1.ndim; i++)
            {
                result[i] = v1[i] + v2[i];
            }
            return result;
        }
        public static Vector operator -(Vector v)
        {
            double[] result = new double[v.ndim];
            for (int i = 0; i < v.ndim; i++)
            {
                result[i] = -v[i];
            }
            return new Vector(result);
        }
        public static Vector operator -(Vector v1, Vector v2)
        {
            Vector result = new Vector(v1.ndim);
            for (int i = 0; i < v1.ndim; i++)
            {
                result[i] = v1[i] - v2[i];
            }
            return result;
        }
        public static Vector operator *(Vector v, double d)
        {
            Vector result = new Vector(v.ndim);
            for (int i = 0; i < v.ndim; i++)
            {
                result[i] = v[i] * d;
            }
            return result;
        }
        public static Vector operator *(double d, Vector v)
        {
            Vector result = new Vector(v.ndim);
            for (int i = 0; i < v.ndim; i++)
            {
                result[i] = d * v[i];
            }
            return result;
        }
        public static Vector operator /(Vector v, double d)
        {
            Vector result = new Vector(v.ndim);
            for (int i = 0; i < v.ndim; i++)
            {
                result[i] = v[i] / d;
            }
            return result;
        }
        public static Vector operator /(double d, Vector v)
        {
            Vector result = new Vector(v.ndim);
            for (int i = 0; i < v.ndim; i++)
            {
                result[i] = v[i] / d;
            }
            return result;
        }
        public double GetNorm()
        {
            double result = 0.0;
            for (int i = 0; i < ndim; i++)
            {
                result += vector[i] * vector[i];
            }
            return Math.Sqrt(result);
        }
        public double GetNormSquare()
        {
            double result = 0.0;
            for (int i = 0; i < ndim; i++)
            {
                result += vector[i] * vector[i];
            }
            return result;
        }
        public Matrix ToMatrix()
        {
            return new Matrix(this.ndim, 1).ReplaceCol(this, 0);
        }
        public Matrix GetTranspose()
        {
            return this.ToMatrix().GetTranspose();
        }
        public static implicit operator Matrix(Vector v)
        {
            return v.ToMatrix();
        }
        public static Vector[] GetUnitVectors (int n)
        {
            var unitVectors = new Vector[n];
            for (int i = 0; i < n;i++ )
            {
                unitVectors[i] = new Vector(n);
                unitVectors[i][i] = 1;
            }
            return unitVectors;
        }
    }
}
