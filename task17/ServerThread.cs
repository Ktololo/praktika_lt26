using System;
using System.Collections.Concurrent;
using System.Threading;

public class ServerThread
{
    private readonly BlockingCollection<ICommand> _queue = new BlockingCollection<ICommand>();
    private readonly Thread _thread;
    private readonly CancellationTokenSource _cts = new CancellationTokenSource();
    private bool _softStopRequested = false;
    private bool _hardStopRequested = false;

    public ServerThread()
    {
        _thread = new Thread(Run);
        _thread.Start();
    }

    public void AddCommand(ICommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        if (_hardStopRequested)
            throw new InvalidOperationException("Поток остановлен HardStop");
        _queue.Add(command);
    }

    public void StopSoft()
    {
        _queue.Add(new SoftStopCommand(this));
    }

    public void StopHard()
    {
        _queue.Add(new HardStopCommand(this));
        _cts.Cancel();
    }

    public bool IsAlive => _thread.IsAlive;

    private void Run()
    {
        try
        {
            while (!_hardStopRequested && !_cts.Token.IsCancellationRequested)
            {
                if (_softStopRequested && _queue.Count == 0)
                    break;

                if (_queue.TryTake(out var command, 100, _cts.Token))
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
        }
        catch (OperationCanceledException)
        {

        }
        finally
        {
            _queue.Dispose();
            _cts.Dispose();
        }
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
