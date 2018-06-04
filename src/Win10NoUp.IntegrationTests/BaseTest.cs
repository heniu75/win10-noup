
namespace Win10NoUp.IntegrationTests
{
    public class BaseTest
    {
        public static string RootFolder(string className) => $"{TestSettings.TestFolder}\\{className}";
        public static string SourceFolder(string className, string methodName) => $"{RootFolder(className)}\\{methodName}\\source";
        public static string TargetFolder(string className, string methodName) => $"{RootFolder(className)}\\{methodName}\\target";

        public static void EnsureFolders(string className, string methodName)
        {
            TestSettings.EnsureFolders(TargetFolder(className, methodName));
            TestSettings.EnsureFolders(SourceFolder(className, methodName));
        }

        public static void ClearFolders(string className, string methodName)
        {
            TestSettings.ClearFolder(TargetFolder(className, methodName));
            TestSettings.ClearFolder(SourceFolder(className, methodName));
        }
    }
}
