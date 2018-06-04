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

    public class ConfigureMessageThrottling : BaseMessage
    {
        public ConfigureMessageThrottling(IActorRef sendTo, Func<object, ThrottleDirection> getThrottleDirection)
        {
            SendTo = sendTo;
            GetThrottleDirection = getThrottleDirection;
        }
        public IActorRef SendTo { get; set; }
        public Func<object, ThrottleDirection> GetThrottleDirection { get; set; }
    }

    public class MessageThrottler : ReceiveActor, IWithUnboundedStash
    {
        private const int MaxMessages = 4;
        public IStash Stash { get; set; }

        private ConfigureMessageThrottling _configuration = null;
        //public MessageThrottler(IActorRef sendTo, Func<object, ThrottleDirection> getThrottleDirection)
        public MessageThrottler()
        {
            int messagesInFlight = 0;
            ActorCorrelations correlations = new ActorCorrelations();

            Receive<ConfigureMessageThrottling>((m) => { _configuration = m; });
            Receive<BaseMessage>((msg) =>
            {
                if (_configuration == null)
                {
                    Stash.Stash();
                    return;
                }
                Stash.UnstashAll();

                var instruction = _configuration.GetThrottleDirection(msg);
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
                        _configuration.SendTo.Tell(msg, Self);
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
