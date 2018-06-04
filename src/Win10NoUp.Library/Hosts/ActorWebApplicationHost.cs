//using System;
//using System.Threading.Tasks;
//using Microsoft.Extensions.DependencyInjection;

//namespace Win10NoUp.Library.Hosts
//{
//    public class ActorWebApplicationHost : IApplicationHost, IDisposable
//    {
//        private IServiceProviderFactory _factory;
//        private IActorSystemHost _actorSystemHost;

//        public ActorWebApplicationHost(IServiceProviderFactory factory)
//        {
//            _factory = factory;
//        }

//        public void Run()
//        {
//            // the typed GetService is from an extension in NS Microsoft.Extensions.DependencyInjection;
//            _actorSystemHost = _factory.ServiceProvider.GetService<IActorSystemHost>();
//            _actorSystemHost.Start();
//            _actorSystemHost.Tell("Hello there!");
//            var task = new Task(() =>
//            {
//                Console.WriteLine("Actor system host started in web application host.");
//            });
//            task.Start();
//        }

//        public void Dispose()
//        {
//            if (_actorSystemHost != null)
//            {
//                _actorSystemHost.Stop();
//                _actorSystemHost = null;
//            }
//            if (_factory != null)
//            {
//                _factory.Dispose();
//                _factory = null;
//            }
//        }
//    }
//}
