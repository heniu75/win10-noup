//using System;
//using Microsoft.Extensions.DependencyInjection;

//namespace Win10NoUp.Library.Hosts
//{
//    public interface IServiceProviderFactory : IDisposable
//    {
//        IServiceProvider ServiceProvider { get; }
//    }

//    public class CoreServiceProviderFactory : IServiceProviderFactory
//    {

//        // for use from asp.net core 2 web app
//        public CoreServiceProviderFactory(IServiceCollection services, IConfigureCoreServices[] configureCoreServices)
//        {
//            foreach(var configureService in configureCoreServices)
//                configureService.RegisterTypes(services);
//            ServiceProvider = services.BuildServiceProvider();
//        }

//        // for use from console application
//        public CoreServiceProviderFactory(IConfigureCoreServices[] configureCoreServices) : this(new ServiceCollection(), configureCoreServices)
//        {
//        }

//        public IServiceProvider ServiceProvider { get; private set; }

//        public void Dispose()
//        {
//            if (ServiceProvider != null)
//            {
//                var disposable = ServiceProvider as IDisposable;
//                if (disposable != null)
//                {
//                    disposable.Dispose();
//                    ServiceProvider = null;
//                }
//            }
//        }
//    }
//}
