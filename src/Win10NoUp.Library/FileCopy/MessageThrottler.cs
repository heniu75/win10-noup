using System;
using Akka.Actor;
using Win10NoUp.Library.Messages;

namespace Win10NoUp.Library.FileCopy
{
    public enum ThrottleDirection
    {
        Outbound,
        Inbound,
        Ignore,
    }

    public class MessageThrottler : ReceiveActor, IWithUnboundedStash
    {
        private const int MaxMessages = 4;
        public IStash Stash { get; set; }

        public MessageThrottler(IActorRef sendTo, Func<object, ThrottleDirection> getThrottleDirection)
        {
            int messagesInFlight = 0;
            ActorCorrelations correlations = new ActorCorrelations();

            Receive<BaseMessage>((msg) =>
            {
                var instruction = getThrottleDirection(msg);
                switch (instruction)
                {
                    case ThrottleDirection.Outbound:
                        if (messagesInFlight >= MaxMessages)
                        {
                            Stash.Stash();
                            return;
                        }
                        messagesInFlight++;
                        correlations.Add(msg.CorrelationId, Sender);
                        sendTo.Tell(msg, Self);
                        break;
                    case ThrottleDirection.Inbound:
                        var requestor = correlations[msg.CorrelationId];
                        requestor.Tell(msg);
                        messagesInFlight--;
                        correlations.Remove(msg.CorrelationId);
                        Stash.Unstash();
                        break;
                }
            });
        }
    }
}
