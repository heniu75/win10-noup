using Akka.Actor;
using Akka.DI.Core;
using Microsoft.Extensions.Logging;
using Win10NoUp.Library.FileCopy;

namespace Win10NoUp.Library
{
    public class ApplicationManagerActor : UntypedActor
    {
        private readonly IFileSystem _fileSystem;
        private readonly ILogger<ApplicationManagerActor> _logger;
        private IActorRef _pubSubActor;
        private IActorRef _fileCopyManager;
        private IActorRef _repeatActionManager;

        public ApplicationManagerActor(IFileSystem fileSystem, ILogger<ApplicationManagerActor> logger)
        {
            _fileSystem = fileSystem;
            _logger = logger;
        }

        protected override void PreStart()
        {
            _pubSubActor = Context.ActorOf(Context.DI().Props<PubSubActor>(), $"{nameof(PubSubActor)}");
            _fileCopyManager = Context.ActorOf(Context.DI().Props<FileCopyManager>(), $"{nameof(FileCopyManager)}");
            _repeatActionManager = Context.ActorOf(Context.DI().Props<RepeatActionManager>(), $"{nameof(RepeatActionManager)}");
        }

        protected override void OnReceive(object message)
        {
            _logger.LogInformation($"Actor: I am {Self.Path.Name}");
            _logger.LogInformation(message.ToString());
        }
    }
}
