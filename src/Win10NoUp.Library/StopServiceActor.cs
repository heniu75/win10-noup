using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using Akka.Actor;
using Microsoft.Extensions.Configuration;
using Win10NoUp.Library.Config;
using Win10NoUp.Library.Messages;

namespace Win10NoUp.Library
{
    public class StopServiceMessage : BaseMessage { }
    public class StopServiceActor : ReceiveActor
    {
        public StopServiceActor(IApplicationConfig config)
        {
            Context.System.Scheduler.ScheduleTellRepeatedly(
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(5),
                Self,
                new StopServiceMessage(),
                Self);

            Receive<StopServiceMessage>((m) =>
            {
                var servicesToStop = config.ServicesToStop;
                ServiceController service = new ServiceController("ServiceName");
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped);
            });
        }
    }
}
