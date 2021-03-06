using System.IO;

namespace Win10NoUp.Library
{
    public interface IFileSystem
    {
        void Copy(string sourceFile, string targetFolder, bool overwrite);
    }

    public class FileSystem : IFileSystem
    {
        public static int Idx = 0;
        public int MyIdx = Idx++;
        public void Copy(string sourceFile, string targetFolder, bool overwrite)
        {
            var target = Path.Combine(targetFolder, Path.GetFileName(sourceFile));
            File.Copy(sourceFile, target, overwrite);
        }
    }
}
