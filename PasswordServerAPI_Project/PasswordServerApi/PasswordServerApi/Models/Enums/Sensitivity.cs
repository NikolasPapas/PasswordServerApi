using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.Models.Enums
{
	public enum Sensitivity
	{
		OnlyUser =0,
		Family =1,
		Friends=2,
		ForGroup = 3,
		ForEveryone = 4,
	}
}
