using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Win10NoUp.Library.Attributes;
using Win10NoUp.Library.Reflection;

namespace Win10NoUp.Library.Hosts
{
    public interface IAutoConfiguration
    {
        void AddAutoConfiguration(IServiceCollection coreServices, IConfiguration configuration);
    }

    public class AutoConfiguration : IAutoConfiguration
    {
        public void AddAutoConfiguration(IServiceCollection coreServices, IConfiguration configuration)
        {
            var autoConfigurationTypes = new AllAttributedTypes<AutoConfigurationDtoAttribute>();
            foreach (var type in autoConfigurationTypes.Types)
            {
                // usually you would do something like the following to configure a config
                // coreServices.Configure<ApplicationConfig>(configuration.GetSection($"{nameof(ApplicationConfig)}"));

                var typedConfigSection = configuration.GetSection($"{type.Name}");
                ReflectionUtil.InvokeGenericMethod(
                    typeof(OptionsConfigurationServiceCollectionExtensions), type,
                    "Configure", coreServices, typedConfigSection);
            }
        }
    }
}