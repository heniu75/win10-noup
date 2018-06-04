using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Configuration;

namespace Win10NoUp.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            //var host1 = new WebHostBuilder()
            //    .UseContentRoot(Directory.GetCurrentDirectory())
            //    .UseStartup<Startup>()
            //    .UseServer(new NoopServer())
            //    .Build();

            //var he = new HostingEnvironment();
            var host = WebHost.CreateDefaultBuilder(args)
                    .UseStartup<Startup>()
                .Build();

            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json");

            //var configurationRoot = builder.Build();

            return host;
        }
    }
}
