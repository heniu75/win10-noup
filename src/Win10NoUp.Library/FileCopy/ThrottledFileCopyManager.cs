//using System;
//using Akka.Actor;
//using Akka.Event;
//using Win10NoUp.Library.Actors;
//using Win10NoUp.Library.Messages;

//namespace Win10NoUp.Library.FileCopy
//{
//    public abstract class TIhrottledFileCopyManager : ReceiveActor
//    {
//    }

//    public class ThrottledFileCopyManager : TIhrottledFileCopyManager
//    {
//        private readonly IFileSystem _fileSystem;
//        private readonly ILoggingAdapter _log = Context.GetLogger();
//        private readonly ThrottlingManager<BaseMessage> _manager = new ThrottlingManager<BaseMessage>();

//        public const int ReceivePingSeconds = 5;

//        public ThrottledFileCopyManager(IFileSystem fileSystem)
//        {
//            _fileSystem = fileSystem;
//            Receive<FileCopyMessage>((m) => ActionFileCopyMessage(m));
//            Receive<FileCopySuccessMessage>((m) => HandleFileCopySuccessMessage(m));
//            Receive<FileCopyFailMessage>((m) => HandleFileCopyFailMessage(m));
//            Receive<PingMessage>((m) => ManagePendingOperations());

//            Context.System.Scheduler.ScheduleTellRepeatedly(TimeSpan.FromSeconds(0),
//                TimeSpan.FromSeconds(ReceivePingSeconds), Self, new PingMessage(), ActorRefs.NoSender);
//        }

//        private void ActionFileCopyMessage(FileCopyMessage msg, IActorRef sender = null)
//        {
//            if (_manager.AddMessage(msg, sender ?? Sender) == ThrottlingAction.MessageEnQueued)
//            {
//                var props = Props.Create<FileCopyActor>(msg.Instruction, msg.CorrelationId, this, _fileSystem);
//                var actor = Context.ActorOf(props, $"{nameof(FileCopyActor)}-{msg.CorrelationId}");
//            }
//        }

//        private void HandleFileCopySuccessMessage(FileCopySuccessMessage msg)
//        {
//            _manager.RemoveMessage(msg);
//        }

//        private void HandleFileCopyFailMessage(FileCopyFailMessage msg)
//        {
//            _manager.RemoveMessage(msg);
//        }

//        private void ManagePendingOperations()
//        {
//            if (_manager.ManagePendingOperations() == ThrottlingAction.MessageEnQueued)
//            {
//                var props = Props.Create<FileCopyActor>(msg.Instruction, msg.CorrelationId, this, _fileSystem);
//                var actor = Context.ActorOf(props, $"{nameof(FileCopyActor)}-{msg.CorrelationId}");
//            }

//        }
//    }
//}
