using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOptimization
{
    public class Optimization
    {
        public delegate double Function(double x);
        public static double theta = (Math.Sqrt(5) + 1) / 2;
        public static double GoldenSection(Function f, double a, double b, double epsilon, int maxIterations)
        {
            int i = 0;
            while (true)
            {
                double x1 = b - (b - a) / theta;
                double x2 = a + (b - a) / theta;
                if (f(x1) >= f(x2))
                    a = x1;
                else
                    b = x2;
                if (Math.Abs(b - a) < epsilon || i > maxIterations)
                    return (a + b) / 2;
                i++;
            }

        }
        public static double Dichotomy(Function f, double a, double b, double epsilon, int maxIterations)
        {
            double x = 0;
            for (x = 0; Math.Abs(b - a) > epsilon; x = (a + b) / 2)
            {
                double d = (b - a) / 4;
                if (f(x - d) < f(x + d))
                    b = x;
                else
                    a = x;
            }
            return x;

        }

    }
}
