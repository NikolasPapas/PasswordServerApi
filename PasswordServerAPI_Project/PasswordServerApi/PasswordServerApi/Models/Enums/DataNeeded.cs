using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.Models.Enums
{
	public enum DataNeeded
	{
		None = 0,
		Account = 1,
		Password = 2,
        All=3,
	}
}
