using System;
using Autofac;
using Win10NoUp.Library.Reflection;

namespace Win10NoUp.Library.Hosts
{
    public interface IConfigureAutofacServices
    {
        void RegisterTypes(ContainerBuilder containerBuilder);
    }

    public class ConfigureAutofacServices : IConfigureAutofacServices
    {
        public void RegisterTypes(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<FileSystem>().As<IFileSystem>().SingleInstance();
            containerBuilder.RegisterType<StopServiceActor>().SingleInstance();

            containerBuilder.RegisterGeneric(typeof(AllTypes<>)).AsSelf();
            containerBuilder.RegisterGeneric(typeof(AllInstances<>)).AsSelf();

            // register the activator func, used in AllInstances' ctor
            containerBuilder.Register<Func<Type, object>>((c, p) =>
            {
                // see https://stackoverflow.com/questions/20583339/autofac-and-func-factories
                var context = c.Resolve<IComponentContext>();
                return (Type type) =>
                {
                    return context.Resolve(type);
                };
            });

            containerBuilder.RegisterGeneric(typeof(AllTypeInstances<>)).AsSelf();

            // test IOC configuration for runtime error detection
            containerBuilder.RegisterType<MyAutofacTransientService>().As<IMyAutofacTransientService>();
            containerBuilder.RegisterType<MyAutofacSingletonService>().As<IMyAutofacSingletonService>().SingleInstance();

            //containerBuilder.Register()
            //containerBuilder.Register<IConnection>((c, p) =>
            //    {
            //        var type = p.TypedAs<ConnectionType>();
            //        switch (type)
            //        {
            //            case ConnectionType.Ssh:
            //                return new SshConnection();
            //            case ConnectionType.Telnet:
            //                return new TelnetConnection();
            //            default:
            //                throw new ArgumentException("Invalid connection type");
            //        }
            //    })
            //    .As<IConnection>();
        }
    }
}