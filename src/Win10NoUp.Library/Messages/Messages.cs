using System;
using System.Collections.Generic;
using Akka.Actor;

namespace Win10NoUp.Library.Messages
{
    public class SubscribeToMessage : Message
    {
        private readonly Type _messageType;
        private readonly IActorRef _actor;

        public SubscribeToMessage(Type MessageType, IActorRef actor)
        {
            _messageType = MessageType;
            _actor = actor;
        }

        public Type MessageType { get { return _messageType; } }

        public IActorRef Actor { get { return _actor; } }
    }

    public class MessagesGroup
    {
        private readonly List<Type> _Messages = new List<Type>();

        public MessagesGroup()
        { }

        public MessagesGroup(IEnumerable<Type> Messages)
        {
            _Messages.AddRange(Messages);
        }

        public List<Type> Messages { get { return _Messages; } }
    }


    public class SubscribeToMessages : Message
    {
        private readonly MessagesGroup _group;
        private readonly IActorRef _actor;

        public SubscribeToMessages(MessagesGroup group, IActorRef actor)
        {
            _group = @group;
            _actor = actor;
        }

        public MessagesGroup MessagesGroup { get { return _group; } }

        public IActorRef Actor { get { return _actor; } }
    }


    public class UnsubscribeFromMessage
    {
        private readonly Type _MessageType;
        private readonly IActorRef _actor;

        public UnsubscribeFromMessage(Type MessageType, IActorRef actor)
        {
            _MessageType = MessageType;
            _actor = actor;
        }

        public Type MessageType { get { return _MessageType; } }

        public IActorRef Actor { get { return _actor; } }
    }

    public class Message
    {
        public string CorrelationId { get; set; }
    }

    public class BaseMessage : Message
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }


    public class UnsubscribeFromMessages : Message
    {
        private readonly MessagesGroup _group;
        private readonly IActorRef _actor;

        public UnsubscribeFromMessages(MessagesGroup group, IActorRef actor)
        {
            _group = @group;
            _actor = actor;
        }

        public MessagesGroup MessageGroup { get { return _group; } }

        public IActorRef Actor { get { return _actor; } }
    }
}
