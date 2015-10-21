using System;
using System.Diagnostics;

namespace NOptimization
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var nelderMeadResult1 = new NelderMead(new Vector(10, 9), -20, 1E-15).Execute(Function.f1, 1000);
            var nelderMeadResult2 = new NelderMead(new Vector(10, 9), -20, 1E-15).Execute(Function.f2, 1000);
            var gradientResult1 = new GradientMethod(1E-15).Execute(Function.f1, Gradient.f1, Optimization.GoldenSection, new Vector(10, 10), 10000);
            var gradientResult2 = new GradientMethod(double.Epsilon).Execute(Function.f2, Gradient.f2, Optimization.GoldenSection, new Vector(0, 0), 100000);
            var conjugateGradientResult1 = new ConjugateGradientMethod(double.Epsilon, new Vector(2, 0)).Execute(Function.f1, Gradient.f1, Optimization.GoldenSection, 1000);
            var conjugateGradientResult2 = new ConjugateGradientMethod(double.Epsilon, new Vector(0, 0)).Execute(Function.f2, Gradient.f2, Optimization.GoldenSection, 1000);
            var quasiNewtonResult1 = new QuasiNewtonMethod(double.Epsilon, new Vector(10, 10)).Execute(Function.f1, Gradient.f1, Optimization.GoldenSection, 1000);
            var quasiNewtonResult2 = new QuasiNewtonMethod(double.Epsilon, new Vector(10, 10)).Execute(Function.f2, Gradient.f2, Optimization.GoldenSection, 1000);

            var quasiNewtonResult21 = new QuasiNewtonMethod(double.Epsilon, new Vector(0, 0)).Execute(Function.f2, Gradient.f2, Optimization.GoldenSection, 1000);
            var quasiNewtonResult22 = new QuasiNewtonMethod(1E-15, new Vector(-4, -4)).Execute(Function.f2, Gradient.f2, Optimization.GoldenSection, 1000);
            var quasiNewtonResult23 = new QuasiNewtonMethod(1E-15, new Vector(-3, 3)).Execute(Function.f2, Gradient.f2, Optimization.GoldenSection, 1000);
            var quasiNewtonResult24 = new QuasiNewtonMethod(1E-15, new Vector(-3.7, -3.2)).Execute(Function.f2, Gradient.f2, Optimization.GoldenSection, 1000);
            var nelderMeadResult3 = new NelderMead(new Vector(10, 9, -10), -20, 1E-15).Execute(Function.f3, 10000);
            var gradientResult3 = new GradientMethod(1E-15).Execute(Function.f3, Gradient.f3, Optimization.GoldenSection, new Vector(10, 10, 10), 10000);
            var conjugateGradientResult3 = new ConjugateGradientMethod(double.Epsilon, new Vector(10, 10, 10)).Execute(Function.f3, Gradient.f3, Optimization.GoldenSection, 1000);
            var quasiNewtonResult3 = new QuasiNewtonMethod(double.Epsilon, new Vector(10, 10, 10)).Execute(Function.f3, Gradient.f3, Optimization.GoldenSection, 1000);
            var nelderMeadResult4 = new NelderMead(new Vector(10, 9), -20, 1E-15).Execute(Function.f4, 1000);
            var gradientResult4 = new GradientMethod(1E-15).Execute(Function.f4, Gradient.f4, Optimization.GoldenSection, new Vector(10, 10), 1000);
            var conjugateGradientResult4 = new ConjugateGradientMethod(double.Epsilon, new Vector(10, 10)).Execute(Function.f4, Gradient.f4, Optimization.GoldenSection, 1000);
            var quasiNewtonResult4 = new QuasiNewtonMethod(double.Epsilon, new Vector(10, 10)).Execute(Function.f4, Gradient.f4, Optimization.GoldenSection, 1000);
            Console.WriteLine("{0,50}", "Первая функция");
            PrintResult(nelderMeadResult1);
            PrintResult(gradientResult1);
            PrintResult(conjugateGradientResult1);
            PrintResult(quasiNewtonResult1);
            Console.WriteLine("\n");
            Console.WriteLine("{0,50}", "Вторая функция");
            PrintResult(nelderMeadResult2);
            PrintResult(gradientResult2);
            PrintResult(conjugateGradientResult2);
            PrintResult(quasiNewtonResult2);
            Console.WriteLine("\n");
            Console.WriteLine("{0,50}", "Третья функция");
            PrintResult(nelderMeadResult3);
            PrintResult(gradientResult3);
            PrintResult(conjugateGradientResult3);
            PrintResult(quasiNewtonResult3);
            Console.WriteLine("{0,50}", "Четвертая функция");
            PrintResult(nelderMeadResult4);
            PrintResult(gradientResult4);
            PrintResult(conjugateGradientResult4);
            PrintResult(quasiNewtonResult4);
            Console.WriteLine("{0,50}", "Все минимумы второй функции");
            PrintResult(quasiNewtonResult21);
            PrintResult(quasiNewtonResult22);
            PrintResult(quasiNewtonResult23);
            PrintResult(quasiNewtonResult24);
            Console.ReadLine();
        }
        public static void PrintResult(MethodResult result)
        {
            Console.WriteLine("{0,60}\nВыход по количеству итераций: {1}\nКоличество итераций: {2}", result.MethodName, result.IterationEnd, result.Iterations);
            Console.WriteLine("Значение функции в найденной точке: {0:0}", result.FunctionValue);
            if (!result.IsSimplex)
            {
                Console.WriteLine("Точка минимума: {0}", result.Result[0].ToString());
            }
            else
            {
                for (int i = 0; i < result.Result.Length; i++)
                {
                    Console.WriteLine("{1} вектор симлекса: {0}", result.Result[i].ToString(), i + 1);
                }
            }
            Console.WriteLine("Времени затрачено: {0} мс", result.stopwatch.ElapsedMilliseconds);

            if (!result.IsSimplexMethod)
            {
                Console.WriteLine("Стартовая точка: {0}", result.StartPoint[0].ToString());
            }
            else
            {
                for (int i = 0; i < result.StartPoint.Length; i++)
                {
                    Console.WriteLine("{1} начальный вектор симлекса: {0}", result.StartPoint[i].ToString(), i + 1);
                }
            }
        }
    }
    public static class Function
    {
        public static double f1(Vector v)
        {
            return 100 * (v.Y - v.X * v.X) * (v.Y - v.X * v.X) + 5 * (1 - v.X) * (1 - v.X);
        }
        public static double f2(Vector v)
        {
            return (v.X * v.X + v.Y - 11) * (v.X * v.X + v.Y - 11) + (v.X + v.Y * v.Y - 7) * (v.X + v.Y * v.Y - 7);
        }
        public static double f3(Vector v)
        {
            return 100 * (v.Y - v.X * v.X) * (v.Y - v.X * v.X) + 5 * (1 - v.X) * (1 - v.X) + 5 * (1 - v.Z) * (1 - v.Z);
        }
        public static double f4(Vector v)
        {
            var x = v.X;
            var y = v.Y;
            return x * x + (y - 1) * (y - 1);
        }
    }
    public static class Gradient
    {
        public static Vector f1(Vector v)
        {
            return new Vector(10 * (40 * v.X * v.X * v.X - 40 * v.X * v.Y + v.X - 1), 200 * (v.Y - v.X * v.X));
        }
        public static Vector f2(Vector v)
        {
            return new Vector(2 * (2 * v.X * (v.X * v.X + v.Y - 11) + v.X + v.Y * v.Y - 7), 2 * (v.X * v.X + 2 * v.Y * (v.X + v.Y * v.Y - 7) + v.Y - 11));
        }
        public static Vector f3(Vector v)
        {
            return new Vector(10 * (40 * v.X * v.X * v.X - 40 * v.X * v.Y + v.X - 1), 200 * (v.Y - v.X * v.X), 10 * (v.Z - 1));
        }
        public static Vector f4(Vector v)
        {
            var x = v.X;
            var y = v.Y;
            return new Vector(2 * x, 2 * (y-1));
        }
    }
    public struct MethodResult
    {
        public Vector[] Result;
        public string MethodName;
        public bool IterationEnd;
        public int Iterations;
        public bool IsSimplex;
        public bool IsSimplexMethod;
        public Stopwatch stopwatch;
        public Vector[] StartPoint;
        public double FunctionValue;
    }
}
