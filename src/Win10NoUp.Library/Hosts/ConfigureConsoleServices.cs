using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Win10NoUp.Library.Config;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace Win10NoUp.Library.Hosts
{
    public class ConsoleConfiguration : IConfiguration
    {
        private readonly IConfigurationRoot _configurationRoot;
        public ConsoleConfiguration(IHostingEnvironment hostingEnv)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{hostingEnv.EnvironmentName}.json", true, true);

            _configurationRoot = builder.Build();
        }

        public IConfigurationSection GetSection(string key)
        {
            return _configurationRoot.GetSection(key);
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return _configurationRoot.GetChildren();
        }

        public IChangeToken GetReloadToken()
        {
            return _configurationRoot.GetReloadToken();
        }

        public string this[string key]
        {
            get => _configurationRoot[key];
            set => _configurationRoot[key] = value;
        }
    }

    public class ConfigureConsoleServices : IConfigureServices
    {
        public void RegisterTypes<T>(T services)
        {
            IServiceCollection coreServices = services as IServiceCollection;
            if (coreServices == null)
                throw new ApplicationException($"Could not configure services for asp.net core. Configuring services using type {nameof(T)} is NotFiniteNumberException supported.");



            coreServices.AddSingleton<IActorSystemHost, ActorSystemHost>();
            coreServices.AddSingleton<IFileSystem, FileSystem>();
            coreServices.AddSingleton<IApplicationConfig, ApplicationConfig>();
            coreServices.AddSingleton<IConfigHelper, ConfigHelper>();
            coreServices.AddTransient<IMyTransientClass, MyTransientClass>();
            coreServices.AddSingleton<IMySingletonClass, MySingletonClass>();


        }
    }
}
