using System.Collections.Generic;
using Akka.Actor;

namespace Win10NoUp.Library
{
    public class ActorCorrelations
    {
        private readonly Dictionary<string, IActorRef> _actorCorrelations = new Dictionary<string, IActorRef>();

        public IActorRef this[string requestId] => _actorCorrelations[requestId];

        public void Add(string requestId, IActorRef requestingActor)
        {
            if (_actorCorrelations.ContainsKey(requestId))
                Remove(requestId);
            _actorCorrelations.Add(requestId, requestingActor);
        }

        public void Remove(string requestId)
        {
            _actorCorrelations.Remove(requestId);
        }
    }

    public class ActorCorrelation
    {
        public ActorCorrelation(int requestId, IActorRef requestingActor)
        {
            RequestId = requestId;
            RequestingActor = requestingActor;
        }

        public int RequestId { get; set; }
        public IActorRef RequestingActor { get; set; }
    }
}
