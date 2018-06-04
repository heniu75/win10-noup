using Autofac;
using Win10NoUp.Library.Hosts;

namespace AkkaDiTest
{
    public interface IConfigureActorServices
    {
        void RegisterTypes(ContainerBuilder containerBuilder);
    }

    public class ConfigureAutofacActorServices : IConfigureActorServices
    {
        public void RegisterTypes(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<FileSystem>().As<IFileSystem>().SingleInstance();
            containerBuilder.RegisterType<StopServiceActor>().SingleInstance();

            containerBuilder.RegisterType<MyAutofacTransientService>().As<IFileSystem>().SingleInstance();
            containerBuilder.RegisterType<StopServiceActor>().SingleInstance();

        }
    }
}