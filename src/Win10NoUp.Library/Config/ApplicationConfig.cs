using Win10NoUp.Library.Attributes;

namespace Win10NoUp.Library.Config
{
    [AutoConfigurationDto]
    public class ApplicationConfig
    {
        public string SourceFolder { get; set; }
        public string TargetFolder { get; set; }
    }

    public class ServiceOperation
    {
        public bool StopService { get; set; }
        public bool DisableService { get; set; }
    }

    public class ServiceToStopDetail
    {
        public int CycleInSeconds { get; set; }
        public string ServiceName { get; set; }
        public ServiceOperation Operations { get; set; }
    }

    [AutoConfigurationDto]
    public class StopServicesConfiguration
    {
        public ServiceToStopDetail[] ServicesToStop { get; set; }
    }
}
