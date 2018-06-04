//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Akka.Actor;
//using Akka.Event;
//using Win10NoUp.Library.Messages;

//namespace Win10NoUp.Library.FileCopy
//{
//    public abstract class IFileCopyManager : UntypedActor
//    {
//    }



//    public class FileCopyManager1 : IFileCopyManager
//    {
//        private readonly IFileSystem _fileSystem;
//        private readonly ILoggingAdapter _log = Context.GetLogger();
//        private Dictionary<string, FileCopyMessage> CurrentOperations { get; }
//        private Dictionary<string, IActorRef> CurrentOperationSenders { get; }
//        private Dictionary<string, FileCopyMessage> PendingOperations { get; }
//        private Dictionary<string, IActorRef> PendingOperationSenders { get; }

//        public const int CurrentOperationsMax = 5;
//        public const int ReceivePingSeconds = 5;

//        public FileCopyManager1(IFileSystem fileSystem)
//        {
//            _fileSystem = fileSystem;
//            CurrentOperations = new Dictionary<string, FileCopyMessage>();
//            PendingOperations = new Dictionary<string, FileCopyMessage>();
//            CurrentOperationSenders = new Dictionary<string, IActorRef>();
//            PendingOperationSenders = new Dictionary<string, IActorRef>();
//            Become(AwaitingInstructions());
//            Context.System.Scheduler.ScheduleTellRepeatedly(TimeSpan.FromSeconds(0),
//                TimeSpan.FromSeconds(ReceivePingSeconds), Self, new PingMessage(), ActorRefs.NoSender);
//        }

//        private UntypedReceive AwaitingInstructions()
//        {
//            return message =>
//            {
//                switch (message)
//                {
//                    case FileCopyMessage msg:
//                        ActionFileCopyMessage(msg, Sender);
//                        break;
//                    case FileCopySuccessMessage msg:
//                        HandleFileCopySuccessMessage(msg);
//                        break;
//                    case FileCopyFailMessage msg:
//                        HandleFileCopyFailMessage(msg);
//                        break;
//                    case PingMessage msg:
//                        ManagePendingOperations();
//                        break;
//                }
//            };
//        }

//        private void ActionFileCopyMessage(FileCopyMessage msg, IActorRef sender)
//        {
//            if (CurrentOperations.Count < CurrentOperationsMax)
//            {
//                if (!CurrentOperations.ContainsKey(msg.CorrelationId))
//                {
//                    CurrentOperations.Add(msg.CorrelationId, msg);
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

//        protected override void OnReceive(object message)
//        {
//            _log.Info(message.ToString());
//        }
//    }
//}
