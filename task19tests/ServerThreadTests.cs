using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;

public class ServerThreadTests
{
    [Fact]
    public void TestCommand_ExecutesThreeTimes()
    {
        var cmd = new TestCommand(1, 3);
        Assert.False(cmd.IsCompleted);

        cmd.Execute();
        cmd.Execute();
        cmd.Execute();

        Assert.True(cmd.IsCompleted);
    }

    [Fact]
    public void ServerThread_ExecutesMultipleTestCommands()
    {
        var scheduler = new RoundRobinScheduler();
        var server = new ServerThread(scheduler);

        var commands = new List<TestCommand>();
        for (int i = 0; i < 5; i++)
        {
            var cmd = new TestCommand(i, 3);
            commands.Add(cmd);
            server.AddLongTask(cmd);
        }

        Thread.Sleep(200);

        foreach (var cmd in commands)
        {
            Assert.True(cmd.IsCompleted);
        }
    }

    [Fact]
    public void RoundRobinScheduler_RotatesThroughTasks()
    {
        var scheduler = new RoundRobinScheduler();
        var cmd1 = new TestCommand(1, 1);
        var cmd2 = new TestCommand(2, 1);

        scheduler.AddLongTask(cmd1);
        scheduler.AddLongTask(cmd2);

        var selected1 = scheduler.Select();
        var selected2 = scheduler.Select();

        Assert.Same(cmd1, selected1);
        Assert.Same(cmd2, selected2);
    }
}