using System;
using Akka.Actor;
using Akka.Configuration;
using Akka.DI.AutoFac;
using Akka.DI.Core;
using Autofac;
using Win10NoUp.Library.Config;

namespace Win10NoUp.Library.Hosts
{
    public interface IActorSystemHost : IStartStop, ISendAnyMessage, IDisposable
    {
        IContainer Container { get; }
    }

    public interface ISendAnyMessage
    {
        void Tell(object anyMessage);
    }

    public class ActorSystemHost<TRootActor> : IActorSystemHost where TRootActor : ActorBase
    {
        private readonly ContainerBuilder _containerBuilder;
        private bool _initialised = false;

        public static int Idx = 0;
        public int MyIdx = Idx++;

        public IContainer Container { get; private set; }
        public IDependencyResolver DependencyResolver { get; private set; }
        public ActorSystem ActorSystem { get; private set; }
        public IActorRef RootActor { get; private set; }

        public ActorSystemHost(ContainerBuilder containerBuilder)
        {
            _containerBuilder = containerBuilder;
        }

        public void Start()
        {
            if (!_initialised)
            {
                var hoconConfig = ConfigurationFactory.ParseString(AkkaHoconConfig.Hocon);
                Container = _containerBuilder.Build();
                ActorSystem = ActorSystem.Create("Win10NoUpActorSystem", hoconConfig);
                DependencyResolver = new AutoFacDependencyResolver(Container, ActorSystem);
                RootActor = ActorSystem.ActorOf(DependencyResolver.Create<TRootActor>(), $"rootActor-{nameof(TRootActor)}");
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
