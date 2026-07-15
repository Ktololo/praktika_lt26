using System;
using System.Threading;

public class ServerThread
{
    private readonly IScheduler _scheduler;
    private readonly Thread _thread;
    private volatile bool _isRunning = true;

    public ServerThread(IScheduler scheduler)
    {
        _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
        _thread = new Thread(Run);
        _thread.Start();
    }

    public void AddCommand(ICommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        if (!_isRunning)
            throw new InvalidOperationException("Поток остановлен");

        _scheduler.Add(command);
    }

    public void AddLongTask(ICommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        if (!_isRunning)
            throw new InvalidOperationException("Поток остановлен");

        if (command is RoundRobinScheduler scheduler)
            scheduler.AddLongTask(command);
        else
            _scheduler.Add(command);
    }

    public bool IsAlive => _thread.IsAlive;

    public void Stop()
    {
        _isRunning = false;
    }

    private void Run()
    {
        while (_isRunning)
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
                        Console.WriteLine($"Ошибка выполнения: {ex.Message}");
                    }
                }
            }
            else
            {
                Thread.Sleep(10);
            }
        }
    }
}