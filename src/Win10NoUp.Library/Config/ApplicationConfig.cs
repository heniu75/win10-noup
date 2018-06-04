using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Win10NoUp.Library.Config
{
    public class ApplicationConfig : IApplicationConfig
    {
        public string SourceFolder { get; set; }
        public string TargetFolder { get; set; }

        public string[] ServicesToStop { get; set; }
    }
}
