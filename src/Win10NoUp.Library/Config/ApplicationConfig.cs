using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Win10NoUp.Library.Config
{
    public class ApplicationConfig : IApplicationConfig
    {
        private readonly IConfiguration _configHelper;

        public ApplicationConfig(IConfiguration configHelper)
        {
            _configHelper = configHelper;
        }

        public string SourceFolder { get; set; }
        public string TargetFolder { get; set; }
    }
}
