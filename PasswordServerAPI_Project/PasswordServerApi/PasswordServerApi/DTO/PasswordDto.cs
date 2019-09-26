using PasswordServerApi.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PasswordServerApi.DTO
{
	[DataContract]
	public class PasswordDto
	{
		[DataMember(Name = "passwordId")]
		public Guid PasswordId { get; set; }

		[DataMember(Name = "name")]
		public string Name { get; set; }

		[DataMember(Name = "userName")]
		public string UserName { get; set; }


		/// <summary>
		/// passwoidfgsdjkfsbdkhfvgsbd
		/// skdhfjsodfjsdifkh
		/// sdjhflksdhfnsd;
		/// skdhfiosdfnhlsk
		/// sjkdghfiosldfn
		/// skjdfhsuioejdn
		/// </summary>
		[DataMember(Name = "password")]
		public string Password { get; set; }

		[DataMember(Name = "logInLink")]
		public string LogInLink { get; set; }

		[DataMember(Name = "sensitivity")]
		public Sensitivity Sensitivity { get; set; }

		[DataMember(Name = "strength")]
		public Strength Strength { get; set; }
	}
}
