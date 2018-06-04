using System;
using System.ServiceProcess;
using Akka.Actor;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Win10NoUp.Library.Config;
using Win10NoUp.Library.Messages;

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
        public StopServiceActor(IOptions<StopServicesConfiguration> options, ILogger<StopServiceActor> logger)
        {
            Console.WriteLine("Hello");

            //var logger = loggerFactory.CreateLogger(this.GetType().Name);
            logger.LogDebug($"In ctor()");
            // get the ball rolling - note the exact same message is sent every single time here
            Context.System.Scheduler.ScheduleTellRepeatedly(
                TimeSpan.FromSeconds(options.Value.CycleInSeconds), TimeSpan.FromSeconds(options.Value.CycleInSeconds),
                Self, new StopServiceMessage(), Self);

            Receive<StopServiceMessage>((m) =>
            {
                logger.LogDebug($"Received StopServiceMessage {m.CorrelationId}");
                foreach (var serviceToStop in options.Value.ServicesToStop)
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
                logger.LogDebug($"Processed {m.CorrelationId}");
            });
        }
    }
}
