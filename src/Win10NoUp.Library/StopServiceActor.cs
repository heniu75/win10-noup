using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using Akka.Actor;
using Microsoft.Extensions.Configuration;
using Win10NoUp.Library.Messages;

namespace Win10NoUp.Library
{
    public class StopServiceMessage : BaseMessage { }
    public class StopServiceActor : ReceiveActor
    {
        private readonly IConfiguration _config;

        public StopServiceActor(IConfiguration config)
        {
            _config = config;

            Context.System.Scheduler.ScheduleTellRepeatedly(
                TimeSpan.FromSeconds(1000),
                TimeSpan.FromSeconds(5),
                Self,
                new StopServiceMessage(),
                ActorRefs.NoSender);

            Receive<StopServiceMessage>((m) =>
            {
                var servicesToStop = _config["ServicesToStop"];
                ServiceController service = new ServiceController("ServiceName");
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped);
            });
        }
    }
}
