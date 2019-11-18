using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.Utilitys.Configuration 
{
	public class ConfigurationManager : IConfigurationManager
	{
		private readonly IConfiguration _configuration;

		public ConfigurationManager(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		private IConfigurationSection Getsection(string section)
		{
			return _configuration.GetSection(section);
		}

		public string GetString(string section, string value)
		{
			if (!string.IsNullOrWhiteSpace(section))
				return Getsection(section)[value];
			else
				return _configuration[value];
		}
	}
}
