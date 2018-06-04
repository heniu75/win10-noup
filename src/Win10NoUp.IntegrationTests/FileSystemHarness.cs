using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using Win10NoUp.Library;

namespace Win10NoUp.IntegrationTests
{
    public class FileSystemHarness : IDisposable
    {
        public FileSystemHarness() : this(new StackTrace().GetFrame(1).GetMethod())
        {
        }

        public FileSystemHarness(MethodBase testMethod)
        {
            TestFixture = testMethod.ReflectedType.Name;
            TestName = testMethod.Name;
            BaseTest.EnsureFolders(TestFixture, TestName);
            BaseTest.ClearFolders(TestFixture, TestName);
            Now = DateTime.Now;
            Suffix = $"\\SampleFile-{Now.ToString("yyyy-MM-dd-HH-mm-ss")}.txt";
            SampleFile = $"{BaseTest.SourceFolder(TestFixture, TestName)}{Suffix}";
            TargetFile = $"{BaseTest.TargetFolder(TestFixture, TestName)}{Suffix}";
            SourceFolderSnap = new FolderSnapshot(Path.GetDirectoryName(SampleFile));
            TargetFolderSnap = new FolderSnapshot(Path.GetDirectoryName(TargetFile));

            FileSystem = new FileSystem();
        }

        public string TestFixture { get; set; }

        public FolderSnapshot TargetFolderSnap { get; set; }
        public FileSystem FileSystem { get; private set; }
        public FolderSnapshot SourceFolderSnap { get; set; }

        public string TargetFile { get; set; }
        public string SampleFile { get; set; }
        public string TargetFolder => Path.GetDirectoryName(TargetFile);
        public string SampleFolder => Path.GetDirectoryName(SampleFile);
        public string Suffix { get; set; }
        public string TestName { get; set; }
        public DateTime Now { get; set; }

        public void Dispose()
        {
        }

        public FileSystemHarness WithSampleFile()
        {
            if (File.Exists(SampleFile))
                File.Delete(SampleFile);
            File.WriteAllText(SampleFile, $"{Now} This is some some sample text in sample file.");
            SourceFolderSnap = new FolderSnapshot(Path.GetDirectoryName(SampleFile));
            return this;
        }

        public FileSystemHarness Waitabit()
        {
            Thread.Sleep(50);
            return this;
        }

        public FileSystemHarness WithTargetFile()
        {
            if (File.Exists(TargetFile))
                File.Delete(TargetFile);
            File.WriteAllText(TargetFile, $"{Now} This is some some sample text in target file.");
            TargetFolderSnap = new FolderSnapshot(Path.GetDirectoryName(TargetFile));
            return this;
        }
    }
}
