using System;
using Akka.Actor;
using Akka.Event;

namespace Win10NoUp.Library.FileCopy
{
    public class FileCopyActor : ReceiveActor
    {
        private readonly ILoggingAdapter _log = Context.GetLogger();

        public FileCopyActor(IFileSystem fileSystem)
        {
            Receive<FileCopyMessage>((msg) =>
            {
                Console.WriteLine(
                    $"FileCopyMessage received {msg.CorrelationId} {msg.Instruction.SourceFile} {msg.Instruction.TargetFolder}");
                var timerStart = DateTime.Now;
                try
                {
                    fileSystem.Copy(msg.Instruction.SourceFile,
                        msg.Instruction.TargetFolder,
                        msg.Instruction.Overwrite);
                    Sender.Tell(new FileCopySuccessMessage(msg.CorrelationId, DateTime.Now - timerStart));
                    _log.Debug($"File copied: {msg.CorrelationId} {msg.Instruction.SourceFile} ==> {msg.Instruction.TargetFolder}");
                }
                catch (Exception exc)
                {
                    _log.Error($"Exception: Handling FileCopyMessage: {msg.CorrelationId} {exc.ToString()}");
                    Sender.Tell(new FileCopyFailMessage(msg.CorrelationId, exc));
                }
            });
        }
    }
}
