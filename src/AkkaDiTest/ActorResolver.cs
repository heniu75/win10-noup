//using System;
//using Akka.DI.AutoFac;
//using Akka.DI.Core;
//using Autofac;

//namespace AkkaDiTest
//{
//    public interface IActorResolver : IDisposable
//    {
//        IDependencyResolver Container { get; }
//    }

//    public class ActorResolver : IActorResolver
//    {

//        // for use from asp.net core 2 web app
//        public ActorResolver(IConfigureAutofacServices[] configureActorServices)
//        {
//            var containerBuilder = new ContainerBuilder();
//            foreach(var configureService in configureActorServices)
//                configureService.RegisterTypes(containerBuilder);
//            var container = containerBuilder.Build();
//            ServiceProvider = new AutoFacDependencyResolver(container, );
//        }

//        public IDependencyResolver ServiceProvider { get; private set; }

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
