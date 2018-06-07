using System;
using System.ServiceProcess;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Win10NoUp.Library.Config;
using Win10NoUp.Library.ServiceControl;

namespace Win10NoUp.Library.Actions
{
    public class StopServicesAction : IRepeatAction
    {
        private readonly IOptions<StopServicesConfiguration> _options;
        private readonly ILogger<StopServicesAction> _logger;
        private readonly IServiceControllerService _serviceControllerService;

        public StopServicesAction(IOptions<StopServicesConfiguration> options,
            ILogger<StopServicesAction> logger,
            IServiceControllerService serviceControllerService)
        {
            _options = options;
            _logger = logger;
            _serviceControllerService = serviceControllerService;
        }

        public int CycleInSeconds => 0; //_options.Value.

        public void Execute()
        {
            foreach (var serviceToStop in _options.Value.ServicesToStop)
            {
                try
                {
                    _serviceControllerService.Stop(serviceToStop.ServiceName);
                }
                catch (Exception e)
                {
                    _logger.LogWarning($"Error stopping service {serviceToStop} - {e}");
                    Console.WriteLine(e);
                }
            }
        }

        public IRepeatingAction[] RepeatingActions { get; }
    }
}
