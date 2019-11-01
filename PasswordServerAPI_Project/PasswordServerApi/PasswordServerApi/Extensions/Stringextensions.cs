using PasswordServerApi.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PasswordServerApi.Extensions
{
	public static class Stringextensions
	{
		private static List<Tuple<int, string>> PointsToRegex = new List<Tuple<int, string>>()
		{
			new Tuple<int, string>( 1, @"[!]"),
			new Tuple<int, string>( 1, @"[-]"),
			new Tuple<int, string>( 1, @"[_]"),
			new Tuple<int, string>( 2, @"[#]"),
			new Tuple<int, string>( 2, @"[|]"),
			new Tuple<int, string>( 2, @"[(]"),
			new Tuple<int, string>( 1, @"[)]"),
			new Tuple<int, string>( 3, @"[A-Z]$"),
			new Tuple<int, string>( 2, @"[\\d]"),
			new Tuple<int, string>( -5, @"[ ]$"),
			new Tuple<int, string>( -1, @"[0]$"),
			new Tuple<int, string>( -2, @"[123]$"),
		};

		public static Strength GetPasswordStrength(this string value)
		{
			int points = 0;
			if (!string.IsNullOrWhiteSpace(value))
			{
				if (value.Length < 5)
					points += 1;
				else if (value.Length < 10)
					points += 2;
				else if (value.Length < 15)
					points += 3;
				else if (value.Length > 15)
					points += 5;

				foreach (Tuple<int, string> regexDiscription in PointsToRegex)
				{
					Match match = Regex.Match(value, regexDiscription.Item2, RegexOptions.IgnoreCase);
					if (match.Success)
						points += regexDiscription.Item1;
				}
			}
			
			//26 -20 VeryStrong
			if (points > 19)
				return Strength.VeryStrong;

			//19 - 17 Strong
			if (points > 16)
				return Strength.Strong;

			//16 - 13 Medium
			if (points > 12)
				return Strength.Medium;

			//12 - 10 Medium
			if (points > 9)
				return Strength.Weak;

			if (points < 0)
				return Strength.Danger;

			return Strength.VeryWeak;
		}

		public static Sensitivity GetPasswordSensitivity(this string value)
		{
			if (value == "OnlyUser")
				return Sensitivity.OnlyUser;
			if (value == "Family")
				return Sensitivity.Family;
			if (value == "Friends")
				return Sensitivity.Friends;
			if (value == "ForGroup")
				return Sensitivity.ForGroup;
			return Sensitivity.ForEveryone;
		}
	}
}
