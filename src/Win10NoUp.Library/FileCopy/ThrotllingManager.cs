//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Akka.Actor;
//using Win10NoUp.Library.Messages;

//namespace Win10NoUp.Library.FileCopy
//{
//    public interface IThrottlingManager<TMessage>
//        where TMessage : BaseMessage
//    {
//    }

//    public enum ThrottlingAction
//    {
//        MessageEnQueued,
//        MessagePending,
//        DoNothing,
//        MessageDeQueued,
//    };

//    public class ThrottlingManager<TMessage> : IThrottlingManager<TMessage>
//        where TMessage : BaseMessage
//    {
//        private Dictionary<string, TMessage> CurrentOperations { get; }
//        private Dictionary<string, IActorRef> CurrentOperationSenders { get; }
//        private Dictionary<string, TMessage> PendingOperations { get; }
//        private Dictionary<string, IActorRef> PendingOperationSenders { get; }

//        public const int CurrentOperationsMax = 5;

//        public ThrottlingManager()
//        {
//            CurrentOperations = new Dictionary<string, TMessage>();
//            PendingOperations = new Dictionary<string, TMessage>();
//            CurrentOperationSenders = new Dictionary<string, IActorRef>();
//            PendingOperationSenders = new Dictionary<string, IActorRef>();
//        }

//        public ThrottlingAction AddMessage(TMessage msg, IActorRef sender)
//        {
//            if (CurrentOperations.Count < CurrentOperationsMax)
//            {
//                if (!CurrentOperations.ContainsKey(msg.CorrelationId))
//                {
//                    CurrentOperations.Add(msg.CorrelationId, msg);
//                    CurrentOperationSenders.Add(msg.CorrelationId, sender);
//                    return ThrottlingAction.MessageEnQueued; // the message should be sent out
//                }
//            }
//            else
//            {
//                if (!PendingOperations.ContainsKey(msg.CorrelationId))
//                {
//                    PendingOperations.Add(msg.CorrelationId, msg);
//                    PendingOperationSenders.Add(msg.CorrelationId, sender);
//                    return ThrottlingAction.MessagePending;
//                }
//            }

//            return ThrottlingAction.DoNothing;
//        }

//        public ThrottlingAction RemoveMessage(TMessage msg)
//        {
//            if (CurrentOperations.ContainsKey(msg.CorrelationId))
//            {
//                var sender = CurrentOperationSenders[msg.CorrelationId];
//                var message = CurrentOperations[msg.CorrelationId];
//                CurrentOperations.Remove(msg.CorrelationId);
//                CurrentOperationSenders.Remove(msg.CorrelationId);
//                sender.Tell(message);
//                return ThrottlingAction.MessageDeQueued;
//            }

//            return ThrottlingAction.DoNothing;
//        }

//        public ThrottlingAction ManagePendingOperations()
//        {
//            if (CurrentOperations.Count < CurrentOperationsMax)
//            {
//                if (PendingOperations.Any())
//                {
//                    var correlationId = PendingOperations.Keys.First();
//                    var message = PendingOperations[correlationId];
//                    var sender = PendingOperationSenders[correlationId];
//                    PendingOperations.Remove(correlationId);
//                    PendingOperationSenders.Remove(correlationId);
//                    return AddMessage(message, sender);
//                }
//            }

//            return ThrottlingAction.DoNothing;
//        }
//    }
//}
