using System;
using Akka.Actor;
using Akka.Routing;
using Win10NoUp.Library.Messages;

namespace Win10NoUp.Library.FileCopy
{
    public abstract class IFileCopyManager : ReceiveActor
    {
    }

    public class FileCopyManager : IFileCopyManager
    {
        private readonly IActorRef _workerRouter;
        private readonly IActorRef _messageThrottler;
        private readonly ActorCorrelations _correlations = new ActorCorrelations();

        public const int NumberOfWorkers = 5;

        public FileCopyManager(IFileSystem fileSystem)
        {
            var workerProps = Props.Create(() => new FileCopyActor(fileSystem))
                .WithRouter(new SmallestMailboxPool(NumberOfWorkers));
            _workerRouter = Context.ActorOf(workerProps, "workers");

            Func<object, ThrottleDirection> getMessageDirection = (msg) =>
            {
                if (msg is FileCopyMessage) return ThrottleDirection.Outbound;
                if (msg is FileCopyFailMessage || msg is FileCopySuccessMessage)
                    return ThrottleDirection.Inbound;
                return ThrottleDirection.Ignore;
            };

            var throttlerProps = Props.Create(() => new MessageThrottler(_workerRouter, getMessageDirection));
            _messageThrottler = Context.ActorOf(throttlerProps, $"{nameof(MessageThrottler)}-0");

            Receive<FileCopyMessage>((m) =>
            {
                _correlations.Add(m.CorrelationId, Sender);
                _messageThrottler.Tell(m);
            });

            Action<BaseMessage> handleReturnMsg = (m) =>
            {
                var sendTo = _correlations[m.CorrelationId];
                _correlations.Remove(m.CorrelationId);
                sendTo.Tell(m);
            };
            Receive<FileCopySuccessMessage>(handleReturnMsg);
            Receive<FileCopyFailMessage>(handleReturnMsg);
        }
    }
}
