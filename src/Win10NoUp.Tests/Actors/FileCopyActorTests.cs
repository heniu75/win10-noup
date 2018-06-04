using System;
using System.IO;
using Akka.Actor;
using Akka.TestKit.Xunit2;
using Win10NoUp.Library;
using Win10NoUp.Library.FileCopy;
using Moq;
using Xunit;

namespace Win10NoUp.Tests.Actors
{
    public class FileCopyActorTests : TestKit
    {
        // see https://petabridge.com/blog/how-to-unit-test-akkadotnet-actors-akka-testkit/

        [Fact]
        public void When_FileSystemCopyFails_FileCopyFailMessage_Sent()
        {
            // arrange
            var fci = new FileCopyInstruction { Overwrite = true, SourceFile = "not-there", TargetFolder = "target"};
            var fileSystemMock = new Mock<IFileSystem>();
            fileSystemMock.Setup(x => x.Copy("not-there", "target", true)).Throws<FileNotFoundException>();
            var targetActor = Sys.ActorOf(Props.Create(() => new FileCopyActor(fileSystemMock.Object)));

            // act
            targetActor.Tell(new FileCopyMessage("0", fci));

            // assert
            ExpectMsg<FileCopyFailMessage>(TimeSpan.FromSeconds(1));
            fileSystemMock.VerifyAll();
        }

        [Fact]
        public void When_FileSystemCopySucceeds_FileCopySuccessMessage_Sent()
        {
            // arrange
            var fci = new FileCopyInstruction { Overwrite = true, SourceFile = "source", TargetFolder = "target" };
            var fileSystemMock = new Mock<IFileSystem>();
            fileSystemMock.Setup(x => x.Copy("source", "target", true));
            var targetActor = Sys.ActorOf(Props.Create(() => new FileCopyActor(fileSystemMock.Object)));

            // act
            targetActor.Tell(new FileCopyMessage("0", fci));

            // assert
            ExpectMsg<FileCopySuccessMessage>(TimeSpan.FromSeconds(1));
            fileSystemMock.VerifyAll();
        }
    }
}
