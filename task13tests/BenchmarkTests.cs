using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using task13;
using Xunit;

namespace task13tests
{
    public class BenchmarkTests
    {
        private const double A = -100;
        private const double B = 100;
        private readonly Func<double, double> F = x => Math.Sin(x);
        private readonly double[] Steps = { 1e-1, 1e-2, 1e-3, 1e-4, 1e-5, 1e-6 };
        private readonly int[] Threads = { 1, 2, 4, 8, 16, 32 };
        [Fact]
        public void FindStep()
        {
            double best = 0;
            double err = double.MaxValue;
            foreach (double h in Steps)
            {
                double res = DefiniteIntegral.Solve(A, B, F, h, 1);
                double e = Math.Abs(res - 0);

                if (e < 1e-4 && h > best)
                {
                    best = h;
                    err = e;
                }
            }
            Assert.True(best > 0, "Шаг не найден");
            File.AppendAllText("results.txt", $"Шаг: {best}, ошибка: {err}\n");
        }
        [Fact]
        public void Measure()
        {
            double step = 1e-4;
            var lines = new System.Collections.Generic.List<string>();
            foreach (int t in Threads)
            {
                double ms = 0;
                int runs = 5;

                for (int i = 0; i < runs; i++)
                {
                    var sw = Stopwatch.StartNew();
                    DefiniteIntegral.Solve(A, B, F, step, t);
                    sw.Stop();
                    ms += sw.Elapsed.TotalMilliseconds;
                }

                lines.Add($"{t}\t{ms / runs:F2}");
            }

            File.WriteAllLines("data.txt", lines);
            double single = 0;
            for (int i = 0; i < 5; i++)
            {
                var sw = Stopwatch.StartNew();
                DefiniteIntegral.Solve(A, B, F, step, 1);
                sw.Stop();
                single += sw.Elapsed.TotalMilliseconds;
            }
            single /= 5;

            double bestMulti = lines
                .Select(x => double.Parse(x.Split('\t')[1]))
                .Min();

            double diff = (single - bestMulti) / single * 100;

            string summary = $"Однопоток: {single:F2} мс\n" +
                             $"Многопоток (лучший): {bestMulti:F2} мс\n" +
                             $"Ускорение: {diff:F2}%";

            File.WriteAllText("summary.txt", summary);
            Assert.True(diff >= 15, $"Ускорение {diff:F2}% < 15%");
        }
    }
}