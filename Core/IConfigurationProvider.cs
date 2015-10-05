using System.Collections.Specialized;

namespace Core
{
    public interface IConfigurationProvider
    {
        NameValueCollection AppSettings { get; }
        string GetSetting(string key);
        T GetSetting<T>(string key) where T : struct;
    }
}