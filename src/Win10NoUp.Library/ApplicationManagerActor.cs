using Akka.Actor;
using Akka.Event;
using Microsoft.Extensions.Configuration;
using Win10NoUp.Library.Config;
using Win10NoUp.Library.FileCopy;

namespace Win10NoUp.Library
{
    public class ApplicationManagerActor : UntypedActor
    {
        private readonly IFileSystem _fileSystem;
        private readonly IApplicationConfig _applicationSettings;
        private readonly IConfiguration _configuration;
        private readonly ILoggingAdapter _log = Context.GetLogger();
        private IActorRef _pubSubActor;
        private IActorRef _fileCopyManager;
        private IActorRef _stopServiceActor;

        public ApplicationManagerActor(IFileSystem fileSystem, IApplicationConfig applicationSettings)
        {
            _fileSystem = fileSystem;
            _applicationSettings = applicationSettings;
        }

        protected override void PreStart()
        {
            var pubSubActorProps = Props.Create(() => new PubSubActor());
            _pubSubActor = Context.ActorOf(pubSubActorProps, "pubSubActor");
            var fileCopyManagerProps = Props.Create(() => new FileCopyManager(_fileSystem));
            _fileCopyManager = Context.ActorOf(fileCopyManagerProps, "fileCopyManager");
            var stopServiceActorProps = Props.Create(() => new StopServiceActor(_applicationSettings));
            _stopServiceActor = Context.ActorOf(stopServiceActorProps, "stopServiceActor");
        }

        protected override void OnReceive(object message)
        {
            _log.Info($"Actor: I am {Self.Path.Name}");
            _log.Info(message.ToString());
        }
    }
}
