using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Win10NoUp.Library.Config;

namespace Win10NoUp.Library.Actions
{
    public class StopServicesAction : IRepeatAction
    {
        private readonly IOptions<StopServicesConfiguration> _options;
        private readonly ILogger<StopServicesAction> _logger;

        public StopServicesAction(IOptions<StopServicesConfiguration> options,
            ILogger<StopServicesAction> logger)
        {
            _options = options;
            _logger = logger;
        }

        public int OffsetInSeconds { get; }
        public int CycleInSeconds { get; }

        public void Execute(RepeatMessage message, ILogger logger)
        {
            foreach (var serviceToStop in _options.Value.ServicesToStop)
            {
                try
                {
                    using (ServiceController service = new ServiceController(serviceToStop))
                    {
                        if ((service.Status != ServiceControllerStatus.Stopped) && (service.Status != ServiceControllerStatus.StopPending))
                        {
                            logger.LogDebug($"Stopping {serviceToStop}");
                            service.Stop();
                            service.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 30));
                        }
                        else
                        {
                            logger.LogDebug($"Skipping {serviceToStop} in state {service.Status.ToString()}.");
                        }
                    }
                }
                catch (Exception e)
                {
                    logger.LogWarning($"Error stopping service {serviceToStop} - {e}");
                    Console.WriteLine(e);
                }
            }
        }
    }
}
