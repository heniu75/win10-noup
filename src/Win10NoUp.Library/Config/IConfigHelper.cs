using System;
using System.Collections.Generic;

namespace Win10NoUp.Library.Config
{
    public interface IConfigHelper
    {
        /// <summary>
        /// Retrieve a config value as a string
        /// </summary>
        string ConfigValueAsString(string key, string exceptionMsg);

        string ConfigValueAsString(string key);
        double ConfigValueAsDouble(string key);

        List<string> CommaSeparatedConfigValue(string configValue);

        bool TryGetConfigValueAsString(string key, out string value);

        string ConfigValueAsStringEnsureBackslash(string key);
        /// <summary>
        /// Retrieve a config value and try to convert it to an int
        /// </summary>
        int ConfigValueAsInt(string key, string exceptionMsg = "");
        /// <summary>
        /// Retrieve a config value and try to convert it to a timespan
        /// </summary>
        TimeSpan ConfigValueAsTimeSpan(string key, string exceptionMsg = "");
        /// <summary>
        /// Retrieve a config value and try to convert it to a Guid
        /// </summary>
        Guid ConfigValueAsGuid(string key, string exceptionMsg = "");

        /// <summary>
        /// Retrieve a config value and try to convert it to a bool
        /// </summary>
        bool ConfigValueAsBool(string key);

        bool ConfigValueAsBool(string key, bool defaultValue);

        bool ConfigValueAsBool(string key, string exceptionMsg);

        string ConnectionStringValueAsString(string key, string exceptionMsg);

        string GetConfigAsStringBasedOnHost(string key, string host);

        decimal ConfigValueAsDecimal(string key, string exceptionMsg = "");

        DateTimeOffset ConfigValueAsDatetimeOffset(string key);

        string[] ConfigValueAsStringArray(string key);
    }
}
