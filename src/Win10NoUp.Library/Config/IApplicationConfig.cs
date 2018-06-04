namespace Win10NoUp.Library.Config
{
    public interface IApplicationConfig
    {
        string SourceFolder { get; set; }
        string TargetFolder { get; set; }
        string[] ServicesToStop { get; set; }
    }
}
