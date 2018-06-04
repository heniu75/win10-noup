using System.IO;
using Win10NoUp.Library.Hosts;
using Microsoft.Extensions.Configuration;

namespace Win10NoUp.WinHost
{
    class Program
    {
        static void Main(string[] args)
        {
            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json");

            //var configurationRoot = builder.Build();

            //var host = new WebHostBuilder()
            //    .UseContentRoot(Directory.GetCurrentDirectory())
            //    .UseStartup<Startup>()
            //    .UseServer(new NoopServer())
            //    .Build();

            //using (var servicesFactory = new CoreServiceProviderFactory(new ConfigureCoreServices()))
            //using (var appHost = new ConsoleApplicationHost(servicesFactory))
            //{
            //    appHost.Run();
            //}
        }
    }
}
