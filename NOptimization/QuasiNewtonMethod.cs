using System.Diagnostics;

namespace NOptimization
{
    public class QuasiNewtonMethod
    {
        public delegate double FunctionDelegate(Vector v);
        public delegate Vector GradientDelegate(Vector v);
        public delegate double OptimizationMethod(Optimization.Function f, double a, double b, double epsilon, int maxIterations);
        private Vector currentValue;
        private double epsilon;
        private Matrix hessianMatrix;
        public Stopwatch stopwatch = new Stopwatch();
        private Vector StartVector;
        public QuasiNewtonMethod(double eps, Vector startVector)
        {
            StartVector = startVector;
            epsilon = eps;
            currentValue = startVector;
            int n = startVector.GetVectorSize;
            hessianMatrix = new Matrix(n, n).IdentityMatrix();

        }
        public MethodResult Execute(FunctionDelegate F, GradientDelegate Gradient, OptimizationMethod OptimizationMethod, int MaxIterations)
        {
            stopwatch.Start();
            for (int i = 0; i < MaxIterations; i++)
            {
                var oldGradientValue = Gradient(currentValue);
                Vector newDirection = -(hessianMatrix * oldGradientValue);
                var lambda = OptimizationMethod((alpha) => { return F(currentValue + alpha * newDirection); }, -10, 10, 1E-9, MaxIterations);
                var vector = lambda * newDirection;
                currentValue = vector + currentValue;
                //var functionValue = F(currentValue);
                var gradientValue = Gradient(currentValue);
                if (gradientValue.LengthSquared < epsilon || vector.LengthSquared < epsilon)
                {
                    stopwatch.Stop();
                    return new MethodResult() { Result = new Vector[] { currentValue }, IterationEnd = false, MethodName = "Квазиньютоновский метод", Iterations = i, stopwatch = stopwatch, StartPoint = new Vector[] { StartVector }, FunctionValue = F(currentValue) };
                }
                var matrixU = gradientValue - oldGradientValue;
                var matrixA = (vector * vector.GetTranspose()) / (vector.ToMatrix().GetTranspose() * matrixU)[0, 0];
                var matrixB = -(hessianMatrix * matrixU * matrixU.GetTranspose() * hessianMatrix) / (matrixU.GetTranspose() * hessianMatrix * matrixU)[0, 0];
                hessianMatrix = hessianMatrix + matrixA + matrixB;
            }
            return new MethodResult() { Result = new Vector[] { currentValue }, IterationEnd = true, MethodName = "Квазиньютоновский метод", Iterations = MaxIterations, stopwatch = stopwatch, StartPoint = new Vector[] { StartVector }, FunctionValue = F(currentValue) };
        }
    }
}
