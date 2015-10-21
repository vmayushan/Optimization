using System;
using System.Diagnostics;
using System.Linq;

namespace NOptimization
{
    public class NelderMead
    {
        private double alpha = 1;
        private double beta = 0.5;
        private double gamma = 2;
        private Vector[] UnitVectors;
        private Vector[] simplex;
        private Vector[] initialSimplex;
        private Vector initialVertex;
        private int multiplier;
        public delegate double FunctionDelegate(Vector v);
        private int dimension;
        public Stopwatch stopwatch = new Stopwatch();
        private double epsilon;
        public NelderMead(Vector initialVertex, int multiplier, double eps)
        {
            dimension = initialVertex.GetVectorSize;
            simplex = new Vector[dimension + 1];
            initialSimplex = new Vector[dimension + 1];
            this.initialVertex = initialVertex;
            this.multiplier = multiplier;
            UnitVectors = Vector.GetUnitVectors(dimension);
            epsilon = eps;
            simplex[0] = initialVertex;
            for (int i = 0; i < dimension; i++)
            {
                simplex[i + 1] = simplex[i] + multiplier * UnitVectors[i];
            }
            initialSimplex = simplex.Clone() as Vector[];
        }
        public MethodResult Execute(FunctionDelegate Function, int MaxIterations)
        {
            stopwatch.Start();
            for (int i = 0; i < MaxIterations; i++)
            {
                bool needSqueeze = false;
                bool needCheck = false;
                simplex = (from v in simplex let f = Function(v) orderby f select v).ToArray();
                var centroidVector = (from v in simplex select v).Take(dimension).Aggregate(new Vector(dimension), (result, element) => result + element) / dimension;
                var reflectionVector = centroidVector * (1 + alpha) - simplex[dimension] * alpha;
                var F = (from v in simplex let f = Function(v) orderby f select new VectorValue { Vector = v, Value = f }).ToList();
                var reflection = new VectorValue() { Vector = reflectionVector, Value = Function(reflectionVector) };
                var highest = F[dimension];
                var middle = F[dimension - 1];
                var littlest = F[0];
                if (reflection < littlest)
                {
                    var expansionVector = centroidVector * (1 - gamma) + reflection.Vector * gamma;
                    var expansion = new VectorValue() { Vector = expansionVector, Value = Function(expansionVector) };
                    if (expansion < littlest)
                    {
                        highest = expansion;
                        needCheck = true;
                    }
                    if (middle < expansion && !needCheck && !needSqueeze)
                    {
                        highest = reflection;
                        needCheck = true;
                    }
                }
                if (littlest < reflection && reflection < middle && !needCheck && !needSqueeze)
                {
                    highest = reflection;
                    needCheck = true;
                }
                if (middle < reflection && reflection < highest && !needCheck && !needSqueeze)
                {
                    var reserv = reflection;
                    reflection = highest;
                    highest = reserv;
                    needSqueeze = true;
                }
                if (highest < reflection && !needCheck && !needSqueeze)
                {
                    needSqueeze = true;
                }
                if (!needCheck)
                {
                    var squeezeVector = highest.Vector * beta + centroidVector * (1 - beta);
                    var squeeze = new VectorValue() { Vector = squeezeVector, Value = Function(squeezeVector) };
                    if (squeeze < highest)
                    {
                        highest = squeeze;
                        needCheck = true;
                    }
                    if (highest < squeeze)
                    {
                        middle.Vector = littlest.Vector + (middle.Vector - littlest.Vector) / 2;
                        highest.Vector = littlest.Vector + (highest.Vector - littlest.Vector) / 2;
                    }
                }
                simplex[0] = littlest.Vector;
                simplex[dimension - 1] = middle.Vector;
                simplex[dimension] = highest.Vector;
                if (needCheck)
                {
                    if (SimplexFinish())
                    {
                        stopwatch.Stop();
                        return new MethodResult() { Result = simplex, IterationEnd = false, IsSimplexMethod = true, MethodName = "Метод деформируемого многогранника", Iterations = i, stopwatch = stopwatch, StartPoint = initialSimplex, FunctionValue = Function(simplex[0]) };
                    }
                }
            }
            stopwatch.Stop();
            return new MethodResult() { Result = simplex, IterationEnd = true, MethodName = "Метод деформируемого многогранника", Iterations = MaxIterations, IsSimplex = true, IsSimplexMethod = true, stopwatch = stopwatch, StartPoint = initialSimplex, FunctionValue = double.NaN };
        }
        public bool SimplexFinish()
        {
            var medium = (from v in simplex select v).Aggregate(new Vector(dimension), (result, element) => result + element) / (dimension + 1);
            var deviation = (from v in simplex select (v - medium).LengthSquared).Sum() / (dimension + 1);
            if (Math.Sqrt(deviation) < epsilon)
                return true;
            return false;
        }
    }
    public class VectorValue
    {
        public Vector Vector;
        public double Value;
        public static bool operator >(VectorValue x, VectorValue y)
        {
            if (x.Value > y.Value)
                return true;
            else
                return false;
        }
        public static bool operator <(VectorValue x, VectorValue y)
        {
            if (x.Value < y.Value)
                return true;
            else
                return false;
        }
    }
}
