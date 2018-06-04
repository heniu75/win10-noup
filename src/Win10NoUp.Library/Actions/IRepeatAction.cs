using Microsoft.Extensions.Logging;

namespace Win10NoUp.Library.Actions
{
    public interface IRepeatAction
    {
        int OffsetInSeconds { get; }
        int CycleInSeconds { get; }
        void Execute();
    }
}