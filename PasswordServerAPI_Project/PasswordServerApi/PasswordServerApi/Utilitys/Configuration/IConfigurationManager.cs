using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.Utilitys.Configuration
{
	public interface IConfigurationManager
	{
		string GetString(string section, string value);
	}
}
