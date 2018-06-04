using Akka.Actor;
using Win10NoUp.Library.Messages;

namespace Win10NoUp.Library
{
    public class PubSubActor : ReceiveActor
    {
        private readonly EventDictionary _subscribers = new EventDictionary();
        public PubSubActor()
        {
            Receive<SubscribeToMessage>(msg => _subscribers.Subscribe(msg.MessageType, msg.Actor));
            Receive<SubscribeToMessages>(msg =>
            {
                foreach (var evt in msg.MessagesGroup.Messages)
                {
                    _subscribers.Subscribe(evt, msg.Actor);
                }
            });
            Receive<UnsubscribeFromMessage>(msg => _subscribers.Unsubscribe(msg.MessageType, msg.Actor));
            Receive<UnsubscribeFromMessages>(msg =>
            {
                foreach (var evt in msg.MessageGroup.Messages)
                {
                    _subscribers.Unsubscribe(evt, msg.Actor);
                }
            });

            ReceiveAny(msg =>
            {
                _subscribers.Tell(msg);
            });
        }
    }
}
