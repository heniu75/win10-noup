//using System;
//using Akka.Actor;

//namespace Win10NoUp.Library.FileCopy
//{
//    public class ActorCreator<TOnCreateMessage> : ReceiveActor
//    {
//        private readonly Func<object, IActorRef> _receiverFactory;

//        public ActorCreator(Func<object, IActorRef> receiverFactory)
//        {
//            _receiverFactory = receiverFactory;
//            Receive<TOnCreateMessage>((m) => OnReceive(m));
//        }

//        protected void OnReceive(TOnCreateMessage message)
//        {
//            var actor = _receiverFactory(message);
//            actor.Tell(message);
//        }
//    }
//}
