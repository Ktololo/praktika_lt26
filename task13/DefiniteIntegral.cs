using System;
using System.Threading;

namespace task13
{
    public static class DefiniteIntegral
    {
        public static double Solve(double a, double b, Func<double, double> f, double step, int threads)
        {
            if (threads < 1) throw new ArgumentException("threads > 0");
            if (step <= 0) throw new ArgumentException("step > 0");
            if (a >= b) throw new ArgumentException("a < b");
            double total = b - a;
            double part = total / threads;
            double sum = 0;
            Exception error = null;
            int done = 0;
            object locker = new object();
            for (int i = 0; i < threads; i++)
            {
                int idx = i;
                double left = a + idx * part;
                double right = (idx == threads - 1) ? b : left + part;
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    try
                    {
                        double s = 0;
                        double x = left;
                        while (x < right)
                        {
                            double next = Math.Min(x + step, right);
                            double mid = (x + next) / 2.0;
                            s += f(mid) * (next - x);
                            x = next;
                        }
                        lock (locker) sum += s;
                    }
                    catch (Exception ex)
                    {
                        lock (locker) if (error == null) error = ex;
                    }
                    finally
                    {
                        Interlocked.Increment(ref done);
                    }
                });
            }
            while (done < threads)
                Thread.Sleep(10);
            if (error != null) throw error;
            return sum;
        }
    }
}