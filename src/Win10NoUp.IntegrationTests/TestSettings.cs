using System.IO;

namespace Win10NoUp.IntegrationTests
{
    public class TestSettings
    {
        public static string TestFolder => "c:\\Win10NoUp.tests";

        static TestSettings()
        {
            Initialise();
        }

        public static void Initialise()
        {
            if (!Directory.Exists(TestFolder))
                Directory.CreateDirectory(TestFolder);
        }

        public static void EnsureFolders(string folder)
        {
            Directory.CreateDirectory(folder);
        }

        public static void ClearFolder(string folder)
        {
            var di = new DirectoryInfo(folder);
            foreach (var file in di.GetFiles())
            {
                file.Delete();
            }
        }
    }
}
