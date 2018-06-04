using Akka.Actor;
using Akka.Event;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Win10NoUp.Library.Config;
using Win10NoUp.Library.FileCopy;

namespace Win10NoUp.Library
{
    public class ApplicationManagerActor : UntypedActor
    {
        private readonly IFileSystem _fileSystem;
        private readonly ApplicationConfig _applicationConfig;
        private readonly StopServicesConfiguration _stopServicesConfiguration;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILoggingAdapter _log = Context.GetLogger();
        private IActorRef _pubSubActor;
        private IActorRef _fileCopyManager;
        private IActorRef _stopServiceActor;

        public ApplicationManagerActor(IFileSystem fileSystem, ApplicationConfig applicationConfig, StopServicesConfiguration stopServicesConfiguration, ILoggerFactory loggerFactory)
        {
            _fileSystem = fileSystem;
            _applicationConfig = applicationConfig;
            _stopServicesConfiguration = stopServicesConfiguration;
            _loggerFactory = loggerFactory;
        }

        protected override void PreStart()
        {
            var pubSubActorProps = Props.Create(() => new PubSubActor());
            _pubSubActor = Context.ActorOf(pubSubActorProps, "pubSubActor");
            var fileCopyManagerProps = Props.Create(() => new FileCopyManager(_fileSystem));
            _fileCopyManager = Context.ActorOf(fileCopyManagerProps, "fileCopyManager");
            //var stopServiceActorProps = Props.Create(() => new StopServiceActor(_stopServicesConfiguration, _loggerFactory));
            //_stopServiceActor = Context.ActorOf(stopServiceActorProps, "stopServiceActor");
        }

        protected override void OnReceive(object message)
        {
            _log.Info($"Actor: I am {Self.Path.Name}");
            _log.Info(message.ToString());
        }
    }
}
