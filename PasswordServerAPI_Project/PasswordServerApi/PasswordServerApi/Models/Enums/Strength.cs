using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.Models.Enums
{
	public enum Strength
	{
		VeryStrong = 0,
		Strong = 1,
		Medium = 2,
		Weak = 3,
		VeryWeak = 4,
		Danger = 5
	}
}
