using System.Diagnostics;

namespace NOptimization
{
    public class ConjugateGradientMethod
    {
        public delegate double FunctionDelegate(Vector v);
        public delegate Vector GradientDelegate(Vector v);
        public delegate double OptimizationMethod(Optimization.Function f, double a, double b, double epsilon, int maxIterations);
        private double epsilon;
        private Vector x;
        private Vector StartVector;
        public Stopwatch stopwatch = new Stopwatch();
        public ConjugateGradientMethod(double eps, Vector startVector)
        {
            epsilon = eps;
            x = startVector;
            StartVector = startVector;
        }
        public MethodResult Execute(FunctionDelegate F, GradientDelegate Gradient, OptimizationMethod OptimizationMethod, int MaxIterations)
        {
            stopwatch.Start();
            var antiGradient = -Gradient(x);
            var gradientSquare = antiGradient.LengthSquared;
            for (int i = 0; i < MaxIterations; i++)
            {
                var lambda = OptimizationMethod((alpha) => { return F(x + alpha * antiGradient); }, -10, 10, 1E-9, MaxIterations);
                x = x + lambda * antiGradient;
                var newAntiGradient = -Gradient(x);
                var newGradientSquare = newAntiGradient.LengthSquared;
                var beta = newGradientSquare / gradientSquare;
                if (i % (500) == 0 && i != 0) beta = 0;
                antiGradient = newAntiGradient + beta * antiGradient;
                gradientSquare = newGradientSquare;
                if (gradientSquare < epsilon)
                {
                    stopwatch.Stop();
                    return new MethodResult() { Result = new Vector[] { x }, IterationEnd = false, Iterations = i, MethodName = "Метод сопряженных градиентов", stopwatch = stopwatch , StartPoint = new Vector[] { StartVector}, FunctionValue = F(x)};
                }
            }
            stopwatch.Stop();
            return new MethodResult() { Result = new Vector[] { x }, IterationEnd = true, MethodName = "Метод сопряженных градиентов", Iterations = MaxIterations, stopwatch = stopwatch, StartPoint = new Vector[] { StartVector }, FunctionValue = F(x) };
        }
    }
}
