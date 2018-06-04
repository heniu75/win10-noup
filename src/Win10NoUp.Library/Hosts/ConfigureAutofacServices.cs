using Autofac;

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

            // test IOC configuration for runtime error detection
            containerBuilder.RegisterType<MyAutofacTransientService>().As<IMyAutofacTransientService>();
            containerBuilder.RegisterType<MyAutofacSingletonService>().As<IMyAutofacSingletonService>().SingleInstance();
        }
    }
}