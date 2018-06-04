using System.Diagnostics;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Win10NoUp.Library;
using Win10NoUp.Library.Hosts;

namespace Win10NoUp.Web
{
    public static class StartupAssertions
    {
        public static void DebugAssertIocCorrectlyConfigured(AutofacServiceProvider serviceProvider)
        {
            // classes configured via autofac
            var autofacTransientService0 = serviceProvider.GetService<IMyAutofacTransientService>();
            var autofacTransientService1 = serviceProvider.GetService<IMyAutofacTransientService>();
            var autofacSingletonService0 = serviceProvider.GetService<IMyAutofacSingletonService>();
            var autofacSingletonService1 = serviceProvider.GetService<IMyAutofacSingletonService>();
            Debug.Assert(autofacTransientService0.MyId == 0);
            Debug.Assert(autofacTransientService1.MyId == 1);
            Debug.Assert(autofacSingletonService0.MyId == 0);
            Debug.Assert(autofacSingletonService1.MyId == 0);
            Debug.Assert(autofacSingletonService0 == autofacSingletonService1);

            // classes configured via asp net core services
            var myCoreTransientService0 = serviceProvider.GetService<IMyCoreTransientService>();
            var myCoreTransientService1 = serviceProvider.GetService<IMyCoreTransientService>();
            var myCoreSingletonService0 = serviceProvider.GetService<IMyCoreSingletonService>();
            var myCoreSingletonService1 = serviceProvider.GetService<IMyCoreSingletonService>();
            Debug.Assert(myCoreTransientService0.MyId == 0);
            Debug.Assert(myCoreTransientService1.MyId == 1);
            Debug.Assert(myCoreSingletonService0.MyId == 0);
            Debug.Assert(myCoreSingletonService1.MyId == 0);
            Debug.Assert(myCoreSingletonService0 == myCoreSingletonService1);

            var logFactory = serviceProvider.GetService<ILoggerFactory>();
            var logger0 = serviceProvider.GetService<ILogger<IMyAutofacTransientService>>();
            var logger1 = serviceProvider.GetService<ILogger<StopServiceActor>>();
        }
    }
}
