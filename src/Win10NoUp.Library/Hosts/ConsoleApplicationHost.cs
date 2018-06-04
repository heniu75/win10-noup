using System;
using Microsoft.Extensions.DependencyInjection;

namespace Win10NoUp.Library.Hosts
{
    public interface IApplicationHost
    {
        void Run();
    }

    public class ConsoleApplicationHost : IApplicationHost, IDisposable
    {
        private IServiceProviderFactory _factory;

        public ConsoleApplicationHost(IServiceProviderFactory factory)
        {
            _factory = factory;
        }

        public void Run()
        {
            using (_factory)
            {
                // the typed GetService is from an extension in NS Microsoft.Extensions.DependencyInjection;
                var actorsHost = _factory.ServiceProvider.GetService<IActorSystemHost>();
                actorsHost.Start();
                actorsHost.Tell("Hello there!");
                Console.WriteLine("Press any [Enter] to close the host.");
                Console.ReadLine();
                actorsHost.Stop();
            }

            _factory = null;
        }

        public void Dispose()
        {
            if (_factory != null)
            {
                _factory.Dispose();
                _factory = null;
            }
        }
    }
}
