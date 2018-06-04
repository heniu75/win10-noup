using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Win10NoUp.Library.Config;
using Win10NoUp.Library.Hosts;

namespace Win10NoUp.Library.Actions
{
    public class NoOpAction : IRepeatAction
    {
        private readonly ILogger<NoOpAction> _logger;

        public NoOpAction(Func<MyAutofacTransientService> myFactory)
        {
            var x = myFactory();
            Console.WriteLine(x.MyId);
        }

        public int OffsetInSeconds { get; }
        public int CycleInSeconds { get; }

        public void Execute(RepeatMessage message, ILogger logger)
        {
           // _logger("Hello from no-op");
        }
    }
}
