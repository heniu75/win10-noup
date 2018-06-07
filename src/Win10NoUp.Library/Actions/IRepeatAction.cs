namespace Win10NoUp.Library.Actions
{
    public interface IRepeatingAction
    {
        string ActionName { get; }
        int CycleInSeconds { get; }
        void Execute();
    }

    public interface IRepeatAction
    {
        IRepeatingAction[] RepeatingActions { get; }
    }
}