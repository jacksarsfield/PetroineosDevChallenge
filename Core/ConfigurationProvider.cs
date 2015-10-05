using System;
using System.Collections.Specialized;
using System.Configuration;

namespace Core
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        public NameValueCollection AppSettings
        {
            get { return ConfigurationManager.AppSettings; }
        }

        public string GetSetting(string key)
        {
            return AppSettings[key];
        }

        public T GetSetting<T>(string key) where T : struct 
        {
            return (T) Convert.ChangeType(AppSettings[key], typeof (T));
        }
    }
}