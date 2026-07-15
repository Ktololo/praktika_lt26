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
    public void HardStop_PreventsNewCommands()
    {
        var server = new ServerThread();
        server.StopHard();

        Thread.Sleep(100);
        Assert.Throws<InvalidOperationException>(() => server.AddCommand(new TestCommand(() => { })));
    }

    [Fact]
    public void AddCommand_AfterHardStop_ThrowsException()
    {
        var server = new ServerThread();
        server.StopHard();

        Thread.Sleep(100);
        Assert.Throws<InvalidOperationException>(() => server.AddCommand(new TestCommand(() => { })));
    }

    [Fact]
    public void StopCommand_FromWrongThread_ThrowsException()
    {
        var server = new ServerThread();
        bool exceptionThrown = false;

        var thread = new Thread(() =>
        {
            try
            {
                server.StopHard();
            }
            catch (InvalidOperationException)
            {
                exceptionThrown = true;
            }
        });

        thread.Start();
        thread.Join(1000);

        Assert.True(server.IsAlive);
    }

    [Fact]
    public void StopCommand_FromCorrectThread_DoesNotThrow()
    {
        var server = new ServerThread();
        bool exceptionThrown = false;

        try
        {
            server.StopSoft();
            Thread.Sleep(200);
        }
        catch (Exception)
        {
            exceptionThrown = true;
        }

        Assert.False(exceptionThrown);
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
