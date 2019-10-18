using System.Configuration;

namespace Potestas.Configuration
{
    /// <summary>
    /// Configuration manager class.
    /// </summary>
    public class Configuration : IConfiguration
    {
        public string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}