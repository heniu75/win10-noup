using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;

namespace Win10NoUp.Library.Messages
{
    public class EventDictionary : IEventDictionary
    {
        private readonly Dictionary<Type, HashSet<IActorRef>> _eventDict = new Dictionary<Type, HashSet<IActorRef>>();

        public void Subscribe(Type eventType, IActorRef actor)
        {
            if (!HasSubscribersFor(eventType))
                _eventDict.Add(eventType, new HashSet<IActorRef>());
            if (!_eventDict[eventType].Contains(actor))
                _eventDict[eventType].Add(actor);
        }

        public bool Unsubscribe(Type eventType, IActorRef actor)
        {
            if (!HasSubscribersFor(eventType))
                return false;
            if (!_eventDict[eventType].Contains(actor))
                return false;
            _eventDict[eventType].Remove(actor);
            return true;
        }

        public bool HasSubscribersFor(Type eventType)
        {
            if (!_eventDict.ContainsKey(eventType))
                return false;
            return true;
        }

        public bool Tell(object message)
        {
            var eventType = message.GetType();
            if (!HasSubscribersFor(eventType))
                return false;
            if (!_eventDict[eventType].Any())
                return false;
            foreach (var actor in _eventDict[eventType])
            {
                actor.Tell(message);
            }
            return true;
        }
    }

    public interface IEventDictionary
    {
        bool HasSubscribersFor(Type eventType);
        void Subscribe(Type eventType, IActorRef actor);
        bool Tell(object message);
        bool Unsubscribe(Type eventType, IActorRef actor);
    }
}
