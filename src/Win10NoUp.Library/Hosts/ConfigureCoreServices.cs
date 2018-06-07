using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Win10NoUp.Library.Hosts
{
    public interface IConfigureCoreServices
    {
        void RegisterTypes(IServiceCollection coreServices, IConfiguration configuration);
    }

    public class ConfigureCoreCoreServices : IConfigureCoreServices
    {
        public void RegisterTypes(IServiceCollection coreServices, IConfiguration configuration)
        {
            // Add controllers as services so they'll be resolved.
            coreServices.AddMvc().AddControllersAsServices();

            // logging -- see https://stackoverflow.com/questions/45781873/is-net-core-2-0-logging-broken
            // also see https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-2.1&tabs=aspnetcore2x
            coreServices.AddLogging(builder =>
            {
                builder.AddConfiguration(configuration.GetSection("Logging"))
                    //.AddConsole()
                    .AddProvider(new CustomLoggerProvider(new DateTimeProvider()))
                    .AddDebug();
            });

            // configuration
            // I cant find the bloody extension method for encapsulating into a class into the library cs project, so keep the configuration stuff here
            coreServices.AddOptions();
            new AutoConfiguration().AddAutoConfiguration(coreServices, configuration);

            // services

            // test IOC configuration for runtime error detection
            coreServices.AddTransient<IMyCoreTransientService, MyCoreTransientService>();
            coreServices.AddSingleton<IMyCoreSingletonService, MyCoreSingletonService>();
        }
    }
}