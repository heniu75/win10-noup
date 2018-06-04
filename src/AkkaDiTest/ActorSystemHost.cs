using System;
using Akka.Actor;
using Akka.Configuration;
using Akka.DI.AutoFac;
using Akka.DI.Core;
using Autofac;

namespace AkkaDiTest
{
    public interface IActorSystemHost : IStartStop, ISendAnyMessage, IDisposable
    {
    }

    public interface ISendAnyMessage
    {
        void Tell(object anyMessage);
    }

    public class ActorSystemHost : IActorSystemHost
    {
        private readonly IConfigureActorServices[] _configureActorServices;
        private bool _initialised = false;

        public static int Idx = 0;
        public int MyIdx = Idx++;

        public IDependencyResolver DependencyResolver { get; private set; }
        public ActorSystem ActorSystem { get; private set; }
        public IActorRef RootActor { get; private set; }

        public ActorSystemHost(IConfigureActorServices[] configureActorServices)
        {
            _configureActorServices = configureActorServices;
        }

        public void Start()
        {
            if (!_initialised)
            {
                var hoconConfig = ConfigurationFactory.ParseString(AkkaHoconConfig.Hocon);
                var containerBuilder = new ContainerBuilder();
                foreach (var configureService in _configureActorServices)
                    configureService.RegisterTypes(containerBuilder);
                var container = containerBuilder.Build();
                ActorSystem = ActorSystem.Create("Win10NoUpActorSystem", hoconConfig);
                DependencyResolver = new AutoFacDependencyResolver(container, ActorSystem);
                RootActor = ActorSystem.ActorOf(DependencyResolver.Create<StopServiceActor>(), "stopServiceActor");
                _initialised = true;
            }
        }

        public void Stop()
        {
            if (_initialised)
            {
                if (ActorSystem != null)
                {
                    ActorSystem.Terminate();
                    ActorSystem.Dispose();
                    ActorSystem = null;
                }

                _initialised = false;
            }
        }

        public void Tell(object anyMessage)
        {
            if (_initialised)
            {
                RootActor.Tell(anyMessage);
            }
        }

        public void Dispose()
        {
            Stop();
            if (DependencyResolver != null)
            {
                if (DependencyResolver is IDisposable)
                {
                    var disposable = DependencyResolver as IDisposable;
                    disposable.Dispose();
                    DependencyResolver = null;
                }
            }
        }
    }
}
