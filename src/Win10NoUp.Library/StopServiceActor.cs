using System;
using System.ServiceProcess;
using Akka.Actor;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Win10NoUp.Library.Config;
using Win10NoUp.Library.Messages;
using Win10NoUp.Library.ServiceControl;

namespace Win10NoUp.Library
{
    public class StopServiceMessage : BaseMessage
    {
        public static int Idx { get; private set; }
        public StopServiceMessage()
        {
            CorrelationId = Idx++.ToString();
        }
    }

    public class StopServiceActor : ReceiveActor
    {
        public StopServiceActor(IOptions<StopServicesConfiguration> options, 
            ILogger<StopServiceActor> logger, 
            IServiceControllerService serviceControllerService)
        {
            Console.WriteLine("Hello");

            //var logger = loggerFactory.CreateLogger(this.GetType().Name);
            logger.LogDebug($"In ctor()");
            // get the ball rolling - note the exact same message is sent every single time here
            Context.System.Scheduler.ScheduleTellRepeatedly(
                TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30),
                Self, new StopServiceMessage(), Self);

            Receive<StopServiceMessage>((m) =>
            {
                logger.LogDebug($"Received StopServiceMessage {m.CorrelationId}");
                foreach (var serviceToStop in options.Value.ServicesToStop)
                {
                    try
                    {
                        serviceControllerService.Stop(serviceToStop.ServiceName);
                    }
                    catch (Exception e)
                    {
                        logger.LogWarning($"Error stopping service {serviceToStop} - {e}");
                        Console.WriteLine(e);
                    }
                }
                logger.LogDebug($"Processed {m.CorrelationId}");
            });
        }
    }
}
