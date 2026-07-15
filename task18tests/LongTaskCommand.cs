using System;

public class LongTaskCommand : ICommand
{
    private int _steps;
    private readonly int _totalSteps;

    public LongTaskCommand(int totalSteps)
    {
        _totalSteps = totalSteps;
        _steps = 0;
    }

    public bool IsCompleted => _steps >= _totalSteps;

    public void Execute()
    {
        if (IsCompleted) return;

        _steps++;
        Console.WriteLine($"Шаг {_steps} из {_totalSteps}");
    }
}