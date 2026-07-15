using System;
using System.Threading;
using Xunit;

public class ServerThreadTests
{
    [Fact]
    public void AddCommand_ExecutesCommand()
    {
        var scheduler = new RoundRobinScheduler();
        var server = new ServerThread(scheduler);
        bool executed = false;

        server.AddCommand(new TestCommand(() => executed = true));
        Thread.Sleep(100);

        Assert.True(executed);
    }

    [Fact]
    public void LongTask_ExecutesInSteps()
    {
        var scheduler = new RoundRobinScheduler();
        var server = new ServerThread(scheduler);
        var task = new LongTaskCommand(3);

        server.AddLongTask(task);
        Thread.Sleep(50);
        server.AddCommand(new TestCommand(() => { }));

        Thread.Sleep(200);
        Assert.False(task.IsCompleted);
    }

    [Fact]
    public void RoundRobinScheduler_SelectsCommandsInOrder()
    {
        var scheduler = new RoundRobinScheduler();
        var cmd1 = new TestCommand(() => { });
        var cmd2 = new TestCommand(() => { });

        scheduler.Add(cmd1);
        scheduler.Add(cmd2);

        Assert.Same(cmd1, scheduler.Select());
        Assert.Same(cmd2, scheduler.Select());
        Assert.Null(scheduler.Select());
    }

    [Fact]
    public void ServerThread_StopsWhenStopped()
    {
        var scheduler = new RoundRobinScheduler();
        var server = new ServerThread(scheduler);

        server.Stop();
        Thread.Sleep(100);

        Assert.False(server.IsAlive);
    }

    private class TestCommand : ICommand
    {
        private readonly Action _action;

        public TestCommand(Action action)
        {
            _action = action;
            IsCompleted = false;
        }

        public bool IsCompleted { get; private set; }

        public void Execute()
        {
            _action();
            IsCompleted = true;
        }
    }
}