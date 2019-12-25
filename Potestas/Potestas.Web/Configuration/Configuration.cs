using Potestas.Configuration;
using ICoreConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Potestas.Web.Configuration
{
	public class Configuration : IConfiguration
	{
		private readonly ICoreConfiguration _configuration;

		public Configuration(ICoreConfiguration coreConfiguration)
		{
			_configuration = coreConfiguration;
		}

		public string GetValue(string key) => _configuration[key];
	}
}