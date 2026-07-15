using System;
using System.Collections.Generic;
using System.Threading;

public class ServerThread
{
    private readonly Queue<ICommand> _queue = new Queue<ICommand>();
    private readonly Thread _thread;
    private readonly object _lock = new object();
    private volatile bool _isRunning = true;
    private volatile bool _softStopRequested = false;
    private volatile bool _hardStopRequested = false;

    public ServerThread()
    {
        _thread = new Thread(Run);
        _thread.Start();
    }

    public void AddCommand(ICommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        lock (_lock)
        {
            if (_hardStopRequested || !_isRunning)
                throw new InvalidOperationException("Поток остановлен");

            _queue.Enqueue(command);
            Monitor.Pulse(_lock);
        }
    }

    public void StopSoft()
    {
        AddCommand(new SoftStopCommand(this));
    }

    public void StopHard()
    {
        _hardStopRequested = true;
        lock (_lock)
        {
            Monitor.Pulse(_lock);
        }
    }

    public bool IsAlive => _thread.IsAlive;

    private void Run()
    {
        while (_isRunning && !_hardStopRequested)
        {
            ICommand command = null;

            lock (_lock)
            {
                while (_queue.Count == 0 && !_hardStopRequested)
                {
                    if (_softStopRequested)
                    {
                        _isRunning = false;
                        return;
                    }

                    Monitor.Wait(_lock);
                }

                if (_hardStopRequested)
                    break;

                command = _queue.Dequeue();
            }

            try
            {
                command?.Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка выполнения: {ex.Message}");
            }
        }

        _isRunning = false;
    }

    private class SoftStopCommand : ICommand
    {
        private readonly ServerThread _server;

        public SoftStopCommand(ServerThread server)
        {
            _server = server;
        }

        public void Execute()
        {
            if (Thread.CurrentThread != _server._thread)
                throw new InvalidOperationException("SoftStop должен выполняться в том же потоке");

            _server._softStopRequested = true;
        }
    }

    private class HardStopCommand : ICommand
    {
        private readonly ServerThread _server;

        public HardStopCommand(ServerThread server)
        {
            _server = server;
        }

        public void Execute()
        {
            if (Thread.CurrentThread != _server._thread)
                throw new InvalidOperationException("HardStop должен выполняться в том же потоке");

            _server._hardStopRequested = true;
        }
    }
}
