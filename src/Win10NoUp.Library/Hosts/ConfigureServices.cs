using System;
using System.ComponentModel.Design;
using System.IO;
using Autofac;
using Win10NoUp.Library.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Win10NoUp.Library.Hosts
{
    public interface IConfigureServices
    {
        void RegisterTypes<T>(T services);
    }

    public interface IMySingletonClass
    {
        int MyId { get; set; }
    }

    public class MySingletonClass : IMySingletonClass
    {
        public static int idx = 0;
        public object lockObject = new object();

        public MySingletonClass()
        {
            lock (lockObject)
            {
                MyId = idx++;
            }
        }

        public int MyId { get; set; }
    }

    public interface IMyTransientClass
    {
        int MyId { get; set; }
    }

    public class MyTransientClass : IMyTransientClass
    {
        public static int idx = 0;
        public object lockObject = new object();

        public MyTransientClass()
        {
            lock (lockObject)
            {
                MyId = idx++;
            }
        }

        public int MyId { get; set; }
    }

    public class ConfigureCoreServices : IConfigureServices
    {
        public void RegisterTypes<T>(T services)
        {
            //ServiceProvider sp;
            //sp.GetServices()
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

    //public class ConfigureAutofacServices : IConfigureServices
    //{
    //    public void RegisterTypes<T>(T services)
    //    {
    //        ContainerBuilder builder = services as ContainerBuilder;
    //        if (builder == null)
    //            throw new ApplicationException($"Could not configure services for AutoFac. Configuring services using type {nameof(T)} is NotFiniteNumberException supported.");

    //        builder.RegisterType<ActorSystemHost>().As<IActorSystemHost>().SingleInstance();
    //        builder.RegisterType<FileSystem>().As<IFileSystem>().SingleInstance();
    //        builder.RegisterType<ApplicationConfig>().As<IApplicationConfig>().SingleInstance();
    //        builder.RegisterType<ConfigHelper>().As<IConfigHelper>().SingleInstance();
    //    }
    //}
}
