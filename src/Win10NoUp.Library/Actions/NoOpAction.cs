using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Win10NoUp.Library.Attributes;

namespace Win10NoUp.Library.Actions
{
    [AutoConfigurationDto]
    public class NoOpConfig
    {
        public int CycleInSeconds { get; set; }

    }

    public class NoOpAction : IRepeatAction
    {
        private readonly ILogger<NoOpAction> _logger;
        private readonly IOptions<NoOpConfig> _config;

        public NoOpAction(ILogger<NoOpAction> logger, IOptions<NoOpConfig> config)
        {
            _logger = logger;
            _config = config;
        }

        public int CycleInSeconds => _config.Value.CycleInSeconds;

        public void Execute()
        {
           _logger.LogDebug("Hello from no-op");
        }

        public IRepeatingAction[] RepeatingActions { get; }
    }
}
