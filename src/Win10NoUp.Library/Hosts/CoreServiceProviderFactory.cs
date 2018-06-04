using System;
using Microsoft.Extensions.DependencyInjection;

namespace Win10NoUp.Library.Hosts
{
    public interface IServiceProviderFactory : IDisposable
    {
        IServiceProvider ServiceProvider { get; }
    }

    public class CoreServiceProviderFactory : IServiceProviderFactory
    {

        // for use from asp.net core 2 web app
        public CoreServiceProviderFactory(IServiceCollection services, IConfigureServices[] configureServices)
        {
            foreach(var configureService in configureServices)
                configureService.RegisterTypes(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        // for use from console application
        public CoreServiceProviderFactory(IConfigureServices[] configureServices) : this(new ServiceCollection(), configureServices)
        {
        }

        public IServiceProvider ServiceProvider { get; private set; }

        public void Dispose()
        {
            if (ServiceProvider != null)
            {
                var disposable = ServiceProvider as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                    ServiceProvider = null;
                }
            }
        }
    }
}
