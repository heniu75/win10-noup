using Microsoft.Extensions.DependencyInjection;

namespace Win10NoUp.Library.Hosts
{
    public interface IConfigureCoreServices
    {
        void RegisterTypes(IServiceCollection coreServices);
    }

    public class ConfigureCoreCoreServices : IConfigureCoreServices
    {
        public void RegisterTypes(IServiceCollection coreServices)
        {
            // configuration

            // services
            
            // test IOC configuration for runtime error detection
            coreServices.AddTransient<IMyCoreTransientService, MyCoreTransientService>();
            coreServices.AddSingleton<IMyCoreSingletonService, MyCoreSingletonService>();
        }
    }
}