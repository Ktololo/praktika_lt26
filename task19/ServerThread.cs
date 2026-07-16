using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class ServerThread
{
    private readonly IScheduler _scheduler;
    private readonly CancellationTokenSource _cts = new CancellationTokenSource();
    private readonly Task _task;
    private volatile bool _isRunning = true;
    private volatile bool _hardStopRequested = false;

    public ServerThread(IScheduler scheduler)
    {
        _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
        _task = Task.Run(Run);
    }

    public void AddCommand(ICommand command)
    {
        if (command == null) throw new ArgumentNullException(nameof(command));
        if (_hardStopRequested || !_isRunning)
            throw new InvalidOperationException("Поток остановлен");
        _scheduler.Add(command);
    }

    public void AddLongTask(ICommand command)
    {
        if (command == null) throw new ArgumentNullException(nameof(command));
        if (_hardStopRequested || !_isRunning)
            throw new InvalidOperationException("Поток остановлен");
        _scheduler.AddLongTask(command);
    }

    public void HardStop()
    {
        _hardStopRequested = true;
        _isRunning = false;
        _cts.Cancel();
    }

    public bool IsAlive => !_task.IsCompleted;

    private void Run()
    {
        try
        {
            while (_isRunning && !_hardStopRequested && !_cts.Token.IsCancellationRequested)
            {
                if (_scheduler.HasCommand())
                {
                    var command = _scheduler.Select();
                    if (command != null && !command.IsCompleted)
                    {
                        try
                        {
                            command.Execute();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                        }
                    }
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
        }
        catch (OperationCanceledException)
        {

        }
        finally
        {
            _cts.Dispose();
        }
    }
}