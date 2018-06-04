//using System;

//namespace AkkaDiTest
//{
//    public interface IApplicationHost
//    {
//        void Run();
//    }

//    public class ActorContainerHost : IApplicationHost, IDisposable
//    {
//        //private IServiceProviderFactory _factory;
//        private IDisposable _factory;

//        //public ActorContainerHost(IServiceProviderFactory factory)
//        public ActorContainerHost()
//        {
//          //  _factory = factory;
//        }

//        public void Run()
//        {
//            //using (_factory)
//            {
//                // the typed GetService is from an extension in NS Microsoft.Extensions.DependencyInjection;
//              //  var actorsHost = _factory.ServiceProvider.GetService<IActorSystemHost>();
//                var actorsHost = new ActorSystemHost(new FileSystem());
//                actorsHost.Start();
//                actorsHost.Tell("Hello there!");
//                Console.WriteLine("Press any [Enter] to close the host.");
//                Console.ReadLine();
//                actorsHost.Stop();
//            }

//            _factory = null;
//        }

//        public void Dispose()
//        {
//            if (_factory != null)
//            {
//                _factory.Dispose();
//                _factory = null;
//            }
//        }
//    }
//}
