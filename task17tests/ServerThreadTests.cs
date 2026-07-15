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
            Thread.Sleep(1000);
            executed = true;
        }));

        Thread.Sleep(50);
        server.StopHard();

        int attempts = 0;
        while (server.IsAlive && attempts < 10)
        {
            Thread.Sleep(100);
            attempts++;
        }

        Assert.False(server.IsAlive);
        Assert.False(executed);
    }

    [Fact]
    public void AddCommand_AfterHardStop_ThrowsException()
    {
        var server = new ServerThread();
        server.StopHard();

        Thread.Sleep(50);
        Assert.Throws<InvalidOperationException>(() => server.AddCommand(new TestCommand(() => { })));
    }

    [Fact]
    public void StopCommand_FromWrongThread_ThrowsException()
    {
        var server = new ServerThread();
        bool exceptionThrown = false;
        server.AddCommand(new TestCommand(() =>
        {
            try
            {
                server.StopHard();
            }
            catch (InvalidOperationException)
            {
                exceptionThrown = true;
            }
        }));

        Thread.Sleep(200);
        Assert.True(exceptionThrown);
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
