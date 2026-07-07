using System;
using System.Threading;

namespace task14
{
    public static class DefiniteIntegral
    {
        public static double Solve(double a, double b, Func<double, double> function, double step, int threadNumber)
        {
            if (threadNumber <= 0)
                throw new ArgumentException("Число потоков должно быть больше 0", nameof(threadNumber));
            if (step <= 0)
                throw new ArgumentException("Шаг должен быть больше 0", nameof(step));
            if (a >= b)
                throw new ArgumentException("Нижний предел должен быть меньше верхнего", nameof(a));

            double totalLength = b - a;
            double segmentLength = totalLength / threadNumber;
            double result = 0;
            Exception exception = null;
            int completedThreads = 0;
            object lockObj = new object();

            for (int i = 0; i < threadNumber; i++)
            {
                int index = i;
                double left = a + index * segmentLength;
                double right = (index == threadNumber - 1) ? b : left + segmentLength;

                ThreadPool.QueueUserWorkItem(_ =>
                {
                    try
                    {
                        double sum = 0;
                        double current = left;

                        while (current < right)
                        {
                            double next = Math.Min(current + step, right);
                            double mid = (current + next) / 2.0;
                            sum += function(mid) * (next - current);
                            current = next;
                        }

                        lock (lockObj)
                        {
                            result += sum;
                        }
                    }
                    catch (Exception ex)
                    {
                        lock (lockObj)
                        {
                            if (exception == null)
                                exception = ex;
                        }
                    }
                    finally
                    {
                        Interlocked.Increment(ref completedThreads);
                    }
                });
            }

            while (completedThreads < threadNumber)
            {
                Thread.Sleep(10);
            }

            if (exception != null)
                throw exception;

            return result;
        }
    }
}