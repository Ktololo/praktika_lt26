public interface ICommand
{
    void Execute();
    bool IsCompleted { get; }
}