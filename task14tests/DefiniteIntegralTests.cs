using System;
using task14;
using Xunit;

namespace task14tests
{
    public class DefiniteIntegralTests
    {
        [Fact]
        public void Solve_LinearFunction_ReturnsCorrect()
        {
            Func<double, double> f = x => x;
            double result = DefiniteIntegral.Solve(-1, 1, f, 1e-4, 2);
            Assert.Equal(0, result, 1e-4);
        }

        [Fact]
        public void Solve_SinFunction_ReturnsCorrect()
        {
            Func<double, double> f = x => Math.Sin(x);
            double result = DefiniteIntegral.Solve(-1, 1, f, 1e-5, 8);
            Assert.Equal(0, result, 1e-4);
        }

        [Fact]
        public void Solve_LinearFunctionPositive_ReturnsCorrect()
        {
            Func<double, double> f = x => x;
            double result = DefiniteIntegral.Solve(0, 5, f, 1e-6, 8);
            Assert.Equal(12.5, result, 1e-5);
        }

        [Fact]
        public void Solve_WithOneThread_Works()
        {
            Func<double, double> f = x => x * x;
            double result = DefiniteIntegral.Solve(0, 2, f, 1e-4, 1);
            Assert.Equal(8.0 / 3.0, result, 1e-4);
        }

        [Fact]
        public void Solve_InvalidThreadNumber_Throws()
        {
            Func<double, double> f = x => x;
            Assert.Throws<ArgumentException>(() => DefiniteIntegral.Solve(0, 1, f, 0.1, 0));
        }

        [Fact]
        public void Solve_InvalidStep_Throws()
        {
            Func<double, double> f = x => x;
            Assert.Throws<ArgumentException>(() => DefiniteIntegral.Solve(0, 1, f, 0, 2));
        }

        [Fact]
        public void Solve_InvalidBounds_Throws()
        {
            Func<double, double> f = x => x;
            Assert.Throws<ArgumentException>(() => DefiniteIntegral.Solve(1, 0, f, 0.1, 2));
        }
    }
}