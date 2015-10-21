using System;
using System.Diagnostics;

namespace NOptimization
{
    public class GradientMethod
    {
        public delegate double FunctionDelegate(Vector v);
        public delegate Vector GradientDelegate(Vector v);
        public delegate double OptimizationMethod(Optimization.Function f, double a, double b, double epsilon, int maxIterations);
        private Vector currentVector;
        private Vector oldVector;
        private double epsilon;
        public Stopwatch stopwatch = new Stopwatch();
        private Vector deltaGradient;

        public GradientMethod(double eps)
        {
            epsilon = eps;
        }
        public MethodResult Execute(FunctionDelegate F, GradientDelegate Gradient, OptimizationMethod OptimizationMethod, Vector StartVector, int MaxIterations)
        {
            stopwatch.Start();
            currentVector = StartVector;
            for (int i = 0; i < MaxIterations; i++)
            {
                oldVector = currentVector;
                deltaGradient = Gradient(currentVector);
                var lambda = OptimizationMethod((x) => { return F(currentVector - x * deltaGradient); }, -10, 10, 1E-9, MaxIterations);
                currentVector = currentVector - lambda * deltaGradient;
                if (deltaGradient.LengthSquared < epsilon)
                {
                    stopwatch.Stop();
                    return new MethodResult() { Result = new Vector[] { currentVector }, IterationEnd = false, Iterations = i, MethodName = "Метод градиентного спуска", stopwatch = stopwatch, StartPoint = new Vector[] { StartVector }, FunctionValue = F(currentVector) };
                }
            }
            stopwatch.Stop();
            return new MethodResult() { Result = new Vector[] { currentVector }, IterationEnd = true, MethodName = "Метод градиентного спуска", Iterations = MaxIterations, stopwatch = stopwatch, StartPoint = new Vector[] { StartVector }, FunctionValue = F(currentVector) };
        }
    }
}
