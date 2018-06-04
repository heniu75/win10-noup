using System;
using System.Collections.Generic;
using System.Linq;

namespace Win10NoUp.Library.Config
{
    public class ConfigHelper : IConfigHelper
    {
        #region Interface implementations

        public string ConfigValueAsString(string key, string exceptionMsg)
        {
            return GetConfigValue(key, exceptionMsg);
        }

        public string[] ConfigValueAsStringArray(string key)
        {
            var content = GetConfigValue(key);

            return content.Split(',');
        }

        public string ConfigValueAsString(string key)
        {
            return GetConfigValue(key);
        }

        public List<string> CommaSeparatedConfigValue(string configValue)
        {
            var delimitedEmails = ConfigValueAsString(configValue);
            var emails = new List<string>();
            if (!string.IsNullOrWhiteSpace(delimitedEmails))
            {
                emails = delimitedEmails.Split(',').ToList();
            }
            return emails;
        }

        public bool TryGetConfigValueAsString(string key, out string value)
        {
            var configValue = GetConfigValue(key);
            if (!string.IsNullOrEmpty(configValue))
            {
                value = configValue;
                return true;
            }

            value = null;
            return false;
        }

        public string ConfigValueAsStringEnsureBackslash(string key)
        {
            var path = ConfigValueAsString(key);

            if (!string.IsNullOrEmpty(path) && !path.EndsWith("\\"))
            {
                return path + "\\";
            }
            return path;
        }

        public Guid ConfigValueAsGuid(string key, string exceptionMsg)
        {
            Guid value;

            if (Guid.TryParse(GetConfigValue(key, exceptionMsg), out value))
            {
                return value;
            }

            throw new InvalidCastException(string.Format("Unable to cast config value {0} to Guid!", key));
        }

        public bool ConfigValueAsBool(string key)
        {
            return ConfigValueAsBool(key, string.Empty);
        }

        /// <summary>
        /// This variant is tolerant of missing config and returns the default value instead
        /// </summary>
        /// <returns></returns>
        public bool ConfigValueAsBool(string key, bool defaultValue)
        {
            bool value;
            return bool.TryParse(GetConfigValue(key), out value) ? value : defaultValue;
        }

        public bool ConfigValueAsBool(string key, string exceptionMsg)
        {
            bool value;

            if (bool.TryParse(GetConfigValue(key, exceptionMsg), out value))
            {
                return value;
            }

            throw new InvalidCastException(string.Format("Unable to cast config value {0} to bool!", key));
        }

        public int ConfigValueAsInt(string key, string exceptionMsg)
        {
            int value;

            if (int.TryParse(GetConfigValue(key, exceptionMsg), out value))
            {
                return value;
            }
            throw new InvalidCastException(string.Format("Unable to cast config value {0} to int!", key));
        }

        public double ConfigValueAsDouble(string key)
        {
            double value;

            if (double.TryParse(GetConfigValue(key), out value))
            {
                return value;
            }
            throw new InvalidCastException($"Unable to cast config value {key} to double!");
        }

        public TimeSpan ConfigValueAsTimeSpan(string key, string exceptionMsg)
        {
            TimeSpan value;

            if (TimeSpan.TryParse(GetConfigValue(key, exceptionMsg), out value))
            {
                return value;
            }
            throw new InvalidCastException(string.Format("Unable to cast config value {0} to a timespan!", key));
        }

        public string ConnectionStringValueAsString(string key, string exceptionMsg)
        {
            return GetConnectionStringValue(key, exceptionMsg);
        }

        public string GetConfigAsStringBasedOnHost(string key, string host)
        {
            string sid;
            if (!TryGetConfigValueAsString($"{key}_{host}", out sid))
            {
                sid = ConfigValueAsString(key);
            }
            return sid;
        }

        public decimal ConfigValueAsDecimal(string key, string exceptionMsg = "")
        {
            decimal value;

            if (decimal.TryParse(GetConfigValue(key, exceptionMsg), out value))
            {
                return value;
            }
            throw new InvalidCastException(string.Format("Unable to cast config value {0} to int!", key));
        }

        public DateTimeOffset ConfigValueAsDatetimeOffset(string key)
        {
            DateTimeOffset value;

            if (DateTimeOffset.TryParse(GetConfigValue(key), out value))
            {
                return value;
            }
            throw new InvalidCastException($"Unable to cast config value {key} to DatetimeOffset!");
        }

        #endregion

        #region Private methods

        private string GetConfigValue(string key, string exceptionMsg)
        {
            //var value = ConfigurationManager.AppSettings.Get(key);

            //if (string.IsNullOrWhiteSpace(value))
            //{
            //    var message = string.IsNullOrWhiteSpace(exceptionMsg) ? string.Format("Config value could not be found key: {0}", key) : exceptionMsg;
            //    throw new ConfigurationErrorsException(message);
            //}

            //return value;
            return null;
        }

        private string GetConfigValue(string key)
        {
            //            return ConfigurationManager.AppSettings.Get(key);
            return null;
        }

        private string GetConnectionStringValue(string key, string exceptionMsg)
        {
            //var value = ConfigurationManager.ConnectionStrings[key];

            //if (value == null || string.IsNullOrWhiteSpace(value.ConnectionString))
            //{
            //    var message = string.IsNullOrWhiteSpace(exceptionMsg) ? string.Format("Connection String value could not be found key: {0}", key) : exceptionMsg;
            //    throw new ConfigurationErrorsException(message);
            //}

            //return value.ConnectionString;

            return null;
        }

        #endregion
    }
}
