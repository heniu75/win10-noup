//using System;
//using Akka.Actor;
//using Akka.Event;
//using Win10NoUp.Library.Messages;

//namespace Win10NoUp.Library.FileCopy
//{
//    public abstract class IFileCopyActor : ReceiveActor
//    {

//    }

//    public class FileCopyInstruction
//    {
//        public string SourceFile { get; set; }
//        public string TargetFolder { get; set; }
//        public bool Overwrite { get; set; }
//    }

//    public class FileCopySuccessMessage : BaseMessage
//    {
//        public FileCopySuccessMessage(string correlationId, TimeSpan completedIn)
//        {
//            base.CorrelationId = correlationId;
//            CompletedIn = completedIn;
//        }

//        public TimeSpan CompletedIn { get; set; }
//    }

//    public class FileCopyFailMessage : BaseMessage
//    {
//        public FileCopyFailMessage(string correlationId, Exception exception)
//        {
//            base.CorrelationId = correlationId;
//            Exception = exception;
//        }
//        public Exception Exception { get; set; }
//    }

//    public class FileCopyActor : IFileCopyActor
//    {
//        private readonly ILoggingAdapter _log = Context.GetLogger();

//        public FileCopyActor(FileCopyInstruction instruction,
//            string correlationId,
//            IActorRef requestor,
//            IFileSystem fileSystem)
//        {
//            Become(CopyInProgress(instruction, correlationId, requestor));
//            RunTask(() =>
//            {
//                var timerStart = DateTime.Now;
//                try
//                {
//                    fileSystem.Copy(instruction.SourceFile, instruction.TargetFolder, instruction.Overwrite);
//                    Self.Tell(new FileCopySuccessMessage(correlationId, DateTime.Now - timerStart));
//                }
//                catch (Exception exc)
//                {
//                    Self.Tell(new FileCopyFailMessage(correlationId, exc));
//                }

//                Self.GracefulShutdown();
//            });
//        }

//        private UntypedReceive CopyInProgress(FileCopyInstruction instruction, string correlationId, IActorRef requestor)
//        {
//            return message =>
//            {
//                switch (message)
//                {
//                    case FileCopyFailMessage msg:
//                        requestor.Tell(msg, Self);
//                        break;
//                    case FileCopySuccessMessage msg:
//                        requestor.Tell(msg, Self);
//                        break;
//                }
//            };
//        }
//    }
//}
