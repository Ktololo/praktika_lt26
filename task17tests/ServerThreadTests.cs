using System;
using System.Threading;
using Xunit;

public class ServerThreadTests
{
    [Fact]
    public void AddCommand_ExecutesCommand()
    {
        var server = new ServerThread();
        bool executed = false;

        server.AddCommand(new TestCommand(() => executed = true));
        Thread.Sleep(100);

        Assert.True(executed);
    }

    [Fact]
    public void SoftStop_StopsAfterQueueEmpty()
    {
        var server = new ServerThread();
        bool executed = false;

        server.AddCommand(new TestCommand(() => executed = true));
        server.StopSoft();

        Thread.Sleep(200);
        Assert.False(server.IsAlive);
        Assert.True(executed);
    }

    [Fact]
    public void HardStop_StopsImmediately()
    {
        var server = new ServerThread();
        bool executed = false;

        server.AddCommand(new TestCommand(() =>
        {
            Thread.Sleep(500);
            executed = true;
        }));

        Thread.Sleep(50);
        server.StopHard();

        Thread.Sleep(100);
        Assert.False(server.IsAlive);
        Assert.False(executed);
    }

    [Fact]
    public void StopCommand_FromWrongThread_ThrowsException()
    {
        var server = new ServerThread();
        var exception = Record.Exception(() =>
        {
            var command = new TestCommand(() => { });
            server.AddCommand(command);
        });

        Assert.Null(exception);
    }

    [Fact]
    public void AddCommand_AfterHardStop_ThrowsException()
    {
        var server = new ServerThread();
        server.StopHard();
        Thread.Sleep(50);

        Assert.Throws<InvalidOperationException>(() => server.AddCommand(new TestCommand(() => { })));
    }

    private class TestCommand : ICommand
    {
        private readonly Action _action;

        public TestCommand(Action action)
        {
            _action = action;
        }
        public void Execute()
        {
            _action();
        }
    }
}