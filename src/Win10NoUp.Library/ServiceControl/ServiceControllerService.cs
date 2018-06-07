using System;
using System.ServiceProcess;

namespace Win10NoUp.Library.ServiceControl
{
    public interface IServiceControllerService
    {
        void Stop(string serviceName);
    }
    
    public class ServiceControllerService : IServiceControllerService
    {
        public void Stop(string serviceName)
        {
            using (var controller = new ServiceController(serviceName))
            {
                if ((controller.Status != ServiceControllerStatus.Stopped) && (controller.Status != ServiceControllerStatus.StopPending))
                {
                    controller.Stop();
                    controller.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 30));
                }
            }
        }
    }
}
