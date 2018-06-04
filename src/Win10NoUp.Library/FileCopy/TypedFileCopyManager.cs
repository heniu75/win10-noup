//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Akka.Actor;
//using Akka.Event;
//using Win10NoUp.Library.Actors;

//namespace Win10NoUp.Library.FileCopy
//{
//    public abstract class ITypedFileCopyManager : ReceiveActor
//    {
//    }

//    public class TypedFileCopyManager : ITypedFileCopyManager
//    {
//        private readonly IFileSystem _fileSystem;
//        private readonly ILoggingAdapter _log = Context.GetLogger();
//        private Dictionary<string, FileCopyMessage> CurrentOperations { get; }
//        private Dictionary<string, IActorRef> CurrentOperationSenders { get; }
//        private Dictionary<string, FileCopyMessage> PendingOperations { get; }
//        private Dictionary<string, IActorRef> PendingOperationSenders { get; }

//        public const int CurrentOperationsMax = 5;
//        public const int ReceivePingSeconds = 5;

//        public TypedFileCopyManager(IFileSystem fileSystem)
//        {
//            _fileSystem = fileSystem;
//            CurrentOperations = new Dictionary<string, FileCopyMessage>();
//            PendingOperations = new Dictionary<string, FileCopyMessage>();
//            CurrentOperationSenders = new Dictionary<string, IActorRef>();
//            PendingOperationSenders = new Dictionary<string, IActorRef>();
//            Receive<FileCopyMessage>((m) => ActionFileCopyMessage(m));
//            Receive<FileCopySuccessMessage>((m) => HandleFileCopySuccessMessage(m));
//            Receive<FileCopyFailMessage>((m) => HandleFileCopyFailMessage(m));
//            Receive<PingMessage>((m) => ManagePendingOperations());

//            Context.System.Scheduler.ScheduleTellRepeatedly(TimeSpan.FromSeconds(0),
//                TimeSpan.FromSeconds(ReceivePingSeconds), Self, new PingMessage(), ActorRefs.NoSender);
//        }

//        private void ActionFileCopyMessage(FileCopyMessage msg, IActorRef sender = null)
//        {
//            if (CurrentOperations.Count < CurrentOperationsMax)
//            {
//                if (!CurrentOperations.ContainsKey(msg.CorrelationId))
//                {
//                    CurrentOperations.Add(msg.CorrelationId, msg);
//                    if (sender == null)
//                        sender = Sender;
//                    CurrentOperationSenders.Add(msg.CorrelationId, sender);
//                    var props = Props.Create<FileCopyActor>(msg.Instruction, msg.CorrelationId, this, _fileSystem);
//                    var actor = Context.ActorOf(props, $"{nameof(FileCopyActor)}-{msg.CorrelationId}");
//                }
//            }
//            else
//            {
//                if (!PendingOperations.ContainsKey(msg.CorrelationId))
//                {
//                    PendingOperations.Add(msg.CorrelationId, msg);
//                    PendingOperationSenders.Add(msg.CorrelationId, sender);
//                }
//            }
//        }

//        private void HandleFileCopySuccessMessage(FileCopySuccessMessage msg)
//        {
//            if (CurrentOperations.ContainsKey(msg.CorrelationId))
//            {
//                var sender = CurrentOperationSenders[msg.CorrelationId];
//                var message = CurrentOperations[msg.CorrelationId];
//                CurrentOperations.Remove(msg.CorrelationId);
//                CurrentOperationSenders.Remove(msg.CorrelationId);
//                sender.Tell(message);
//            }
//            ManagePendingOperations();
//        }

//        private void HandleFileCopyFailMessage(FileCopyFailMessage msg)
//        {
//            if (CurrentOperations.ContainsKey(msg.CorrelationId))
//            {
//                var sender = CurrentOperationSenders[msg.CorrelationId];
//                var message = CurrentOperations[msg.CorrelationId];
//                CurrentOperations.Remove(msg.CorrelationId);
//                CurrentOperationSenders.Remove(msg.CorrelationId);
//                sender.Tell(message);
//            }
//            ManagePendingOperations();
//        }

//        private void ManagePendingOperations()
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
//                    ActionFileCopyMessage(message, sender);
//                }
//            }
//        }
//    }
//}
