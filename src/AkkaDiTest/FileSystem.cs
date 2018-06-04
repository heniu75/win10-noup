using System.IO;

namespace AkkaDiTest
{
    public interface IFileSystem
    {
        void Copy(string sourceFile, string targetFolder, bool overwrite);
    }

    public class FileSystem : IFileSystem
    {
        public void Copy(string sourceFile, string targetFolder, bool overwrite)
        {
            var target = Path.Combine(targetFolder, Path.GetFileName(sourceFile));
            File.Copy(sourceFile, target, overwrite);
        }
    }
}
