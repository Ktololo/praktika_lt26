using System.Collections.Generic;

public class RoundRobinScheduler : IScheduler
{
    private readonly Queue<ICommand> _queue = new Queue<ICommand>();
    private readonly List<ICommand> _longTasks = new List<ICommand>();
    private int _currentIndex = 0;

    public void Add(ICommand cmd)
    {
        if (cmd == null) return;

        if (cmd.IsCompleted)
            return;

        _queue.Enqueue(cmd);
    }

    public void AddLongTask(ICommand cmd)
    {
        if (cmd == null) return;
        if (cmd.IsCompleted) return;

        _longTasks.Add(cmd);
    }

    public bool HasCommand()
    {
        if (_queue.Count > 0) return true;
        if (_longTasks.Count == 0) return false;

        foreach (var task in _longTasks)
        {
            if (!task.IsCompleted) return true;
        }

        return false;
    }

    public ICommand Select()
    {
        if (_queue.Count > 0)
            return _queue.Dequeue();
        if (_longTasks.Count == 0)
            return null;

        int attempts = 0;
        while (attempts < _longTasks.Count)
        {
            var cmd = _longTasks[_currentIndex];
            _currentIndex = (_currentIndex + 1) % _longTasks.Count;

            if (!cmd.IsCompleted)
                return cmd;

            attempts++;
        }

        return null;
    }
}