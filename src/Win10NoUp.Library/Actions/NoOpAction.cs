using System;
using Microsoft.Extensions.Logging;
using Win10NoUp.Library.Hosts;

namespace Win10NoUp.Library.Actions
{
    public class NoOpAction : IRepeatAction
    {
        private readonly ILogger<NoOpAction> _logger;

        public NoOpAction(Func<MyAutofacTransientService> myFactory,
            ILogger<NoOpAction> logger)
        {
            _logger = logger;
            var x = myFactory();
            Console.WriteLine(x.MyId);
        }

        public int OffsetInSeconds { get; }
        public int CycleInSeconds { get; }

        public void Execute()
        {
           _logger.LogDebug("Hello from no-op");
        }
    }
}
