public interface IScheduler
{
    bool HasCommand();
    ICommand Select();
    void Add(ICommand cmd);
    void AddLongTask(ICommand cmd);
}