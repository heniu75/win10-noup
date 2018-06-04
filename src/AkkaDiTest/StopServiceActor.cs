using System;
using System.ServiceProcess;
using Akka.Actor;

namespace AkkaDiTest
{
    public class StopServiceMessage 
    {
        public StopServiceMessage()
        {
        }
    }

    public class StopServiceActor : ReceiveActor
    {
        private readonly IFileSystem _fileSystem;

        public StopServiceActor(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            // get the ball rolling - note the exact same message is sent every single time here
            Context.System.Scheduler.ScheduleTellRepeatedly(
                TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30),
                Self, new StopServiceMessage(), Self);

            Receive<StopServiceMessage>((m) =>
            {
                foreach (var serviceToStop in new[] {"ServiceA", "ServiceB"})
                {
                    try
                    {
                        using (ServiceController service = new ServiceController(serviceToStop))
                        {
                            if ((service.Status != ServiceControllerStatus.Stopped) && (service.Status != ServiceControllerStatus.StopPending))
                            {
                                service.Stop();
                                service.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 30));
                            }
                            else
                            {
                                Console.WriteLine("skipping");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            });
        }
    }
}
