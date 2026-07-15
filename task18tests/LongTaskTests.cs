using Xunit;

public class LongTaskTests
{
    [Fact]
    public void LongTask_IsCompleted_WhenStepsReached()
    {
        var task = new LongTaskCommand(3);
        Assert.False(task.IsCompleted);

        task.Execute();
        task.Execute();
        task.Execute();

        Assert.True(task.IsCompleted);
    }

    [Fact]
    public void LongTask_Execute_DoesNothing_WhenCompleted()
    {
        var task = new LongTaskCommand(1);
        task.Execute();
        task.Execute();

        Assert.True(task.IsCompleted);
    }
}