using System;
using Win10NoUp.Library.Messages;

namespace Win10NoUp.Library.FileCopy
{
    public class FileCopyMessage : BaseMessage
    {
        public FileCopyInstruction Instruction { get; }

        public FileCopyMessage(string correlationId, FileCopyInstruction instruction)
        {
            Instruction = instruction;
            base.CorrelationId = correlationId;
        }
    }

    public class PingMessage : BaseMessage
    {
    }

    public class FileCopyInstruction
    {
        public FileCopyInstruction() { }

        public FileCopyInstruction(string sourceFile, string targetFolder, bool overWrite)
        {
            SourceFile = sourceFile;
            TargetFolder = targetFolder;
            Overwrite = overWrite;
        }

        public string SourceFile { get; set; }
        public string TargetFolder { get; set; }
        public bool Overwrite { get; set; }
    }

    public class FileCopySuccessMessage : BaseMessage
    {
        public FileCopySuccessMessage(string correlationId, TimeSpan completedIn)
        {
            base.CorrelationId = correlationId;
            CompletedIn = completedIn;
        }

        public TimeSpan CompletedIn { get; set; }
    }

    public class FileCopyFailMessage : BaseMessage
    {
        public FileCopyFailMessage(string correlationId, Exception exception)
        {
            base.CorrelationId = correlationId;
            Exception = exception;
        }
        public Exception Exception { get; set; }
    }
}
