using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Conjugate_gradient
{
    class Program
    {
        public static double eps = Double.Epsilon;
        static void Main(string[] args)
        {
            int n = 0;
            double[] xk = new double[2] { 0, 0 }; // {2,0} - первая функция
            // {10, 10} - вторая функция
            double[] sk = grad(xk);
            sk[0] = -sk[0]; sk[1] = -sk[1];
            double lambda, fnew = 0, fold = F(xk);
            double[] grad_new = new double[2], grad_old = new double[2];
            while ((Norma(grad(xk)) > eps) & (Norma(sk) > eps))
            {
                n++;
                lambda = Minimize(xk, sk);
                grad_old = grad(xk);
                fold = F(xk);
                xk[0] = xk[0] + sk[0] * lambda; xk[1] = xk[1] + sk[1] * lambda;
                fnew = F(xk); grad_new = grad(xk);
                sk[0] = -grad_new[0] + sk[0] *
                Math.Pow(Norma(grad_new) / Norma(grad_old), 2);
                sk[1] = -grad_new[1] + sk[1] *
                Math.Pow(Norma(grad_new) / Norma(grad_old), 2);
            }
            Console.WriteLine("\nmin = {0}\nin the point = {1} ; {2}\nsteps = {3}", F(xk), xk[0], xk[1], n);
            Console.ReadLine();
        }
        public static double Norma(double[] g)
        {
            return Math.Sqrt(g[0] * g[0] + g[1] * g[1]);
        }
        public static double F(double[] x) // function
        {
            return 100 * Math.Pow((x[1] - x[0] * x[0]), 2) + 5 * Math.Pow(1 - x[0], 2); // - функция 1
            //return Math.Pow(x[0] * x[0] + x[1] - 11, 2) + Math.Pow(x[0] + x[1] * x[1] - 7, 2); // - функция 2
        }
        public static double[] grad(double[] x) // gradient
        {
            double[] grad1 = new double[2];
            // функция 1
            grad1[0] = -400 * x[0] * (x[1] - x[0] * x[0]) - 10 * (1 - x[0]);
            grad1[1] = 200 * (x[1] - x[0] * x[0]);
            //return grad1;
            //функция 2
            //grad1[0] = 4 * x[0] * (x[0] * x[0] + x[1] - 11) + 2 * (x[0] + x[1] * x[1] - 7);
            //grad1[1] = 2 * (x[0] * x[0] + x[1] - 11) + 4 * x[1] * (x[0] + x[1] * x[1] - 7);
            return grad1;
        }
        public static double Minimize(double[] X_Vector, double[] V_Vector)
        {
            double c = (-1 + Math.Sqrt(5)) / 2;
            double a = 0, b = 100;
            double x1 = c * a + (1 - c) * b;
            double fx1 = F(new double[] { X_Vector[0] + x1 * V_Vector[0],
X_Vector[1] + x1 * V_Vector[1] });
            double x2 = (1 - c) * a + c * b;
            double fx2 = F(new double[] { X_Vector[0] + x2 * V_Vector[0],
X_Vector[1] + x2 * V_Vector[1] });
            double epss = 0.0000000001;
            while (Math.Abs(a - b) > epss)
            {
                if (fx1 < fx2)
                {
                    b = x2;
                    x2 = x1;
                    fx2 = fx1;
                    x1 = c * a + (1 - c) * b;
                    fx1 = F(new double[] { X_Vector[0] + x1 * V_Vector[0],
X_Vector[1] + x1 * V_Vector[1] });
                }
                else
                {
                    a = x1;
                    x1 = x2;
                    fx1 = fx2;
                    x2 = (1 - c) * a + c * b;
                    fx2 = F(new double[] { X_Vector[0] + x2 * V_Vector[0],
X_Vector[1] + x2 * V_Vector[1] });
                }
            }
            return (a + b) / 2;
        }
    }
}