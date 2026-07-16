using System;

public class TestCommand : ICommand
{
    private readonly int _id;
    private int _counter = 0;
    private readonly int _maxExecutions;
    private readonly ServerThread _server;

    public TestCommand(int id, int maxExecutions = 3, ServerThread server = null)
    {
        _id = id;
        _maxExecutions = maxExecutions;
        _server = server;
    }

    public bool IsCompleted => _counter >= _maxExecutions;

    public void Execute()
    {
        if (IsCompleted) return;

        if (_server != null && !_server.IsAlive)
        {
            Console.WriteLine($"Поток {_id} остановлен, прерываем выполнение");
            return;
        }

        _counter++;
        Console.WriteLine($"Поток {_id} вызов {_counter}");
    }
}