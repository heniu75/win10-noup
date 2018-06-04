using System;
using Akka.Actor;
using Akka.TestKit.Xunit2;
using Win10NoUp.Library.FileCopy;
using Xunit;

namespace Win10NoUp.IntegrationTests
{
    public class FileCopyTests : TestKit
    {
        [Fact]
        public void FileCopy_SingleFile_Succeeds()
        {
            // arrange
            using (var harness = new FileSystemHarness().WithSampleFile())
            {
                var fci = new FileCopyInstruction(harness.SampleFile, harness.TargetFolder, false);
                var targetActor = Sys.ActorOf(Props.Create(() => new FileCopyActor(harness.FileSystem)));

                // act
                targetActor.Tell(new FileCopyMessage("0", fci));

                // assert
                ExpectMsg<FileCopySuccessMessage>(new TimeSpan(0, 0, 30));
                Assert.True(harness.TargetFolderSnap.ListDifferences());
                Assert.True(harness.TargetFolderSnap.IsNewFile(harness.TargetFile));
            }
        }

        [Fact]
        public void FileCopy_SingleFile_Overwrite_ExistingFile_Succeeds()
        {
            // arrange
            using (var harness = new FileSystemHarness().WithTargetFile().Waitabit().WithSampleFile())
            {
                var fci = new FileCopyInstruction(harness.SampleFile, harness.TargetFolder, true);
                var targetActor = Sys.ActorOf(Props.Create(() => new FileCopyActor(harness.FileSystem)));

                // act
                targetActor.Tell(new FileCopyMessage("0", fci));

                // assert
                ExpectMsg<FileCopySuccessMessage>(new TimeSpan(0, 0, 30));
                Assert.True(harness.TargetFolderSnap.NoListDifferences());
                Assert.False(harness.TargetFolderSnap.IsNewlyListedFile(harness.TargetFile));
                Assert.True(harness.TargetFolderSnap.IsRecentlyModified(harness.TargetFile));
            }
        }

        [Fact]
        public void FileCopy_SingleFile_DoNotOverwrite_ExistingFile_Fails()
        {
            // arrange
            using (var harness = new FileSystemHarness().WithTargetFile().Waitabit().WithSampleFile())
            {
                var fci = new FileCopyInstruction(harness.SampleFile, harness.TargetFolder, false);
                var targetActor = Sys.ActorOf(Props.Create(() => new FileCopyActor(harness.FileSystem)));

                // act
                targetActor.Tell(new FileCopyMessage("0", fci));

                // assert
                ExpectMsg<FileCopyFailMessage>(new TimeSpan(0, 0, 30));
                Assert.True(harness.TargetFolderSnap.NoListDifferences());
                Assert.False(harness.TargetFolderSnap.IsNewlyListedFile(harness.TargetFile));
                Assert.False(harness.TargetFolderSnap.IsRecentlyModified(harness.TargetFile));
            }
        }
    }
}
