using PasswordServerApi.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PasswordServerApi.DataSqliteDB.DataModels
{
		[DataContract]
		public class PasswordModel
		{
			[Required]
			[DataMember(Name = "passwordId")]
			public string PasswordId { get; set; }

			[Required]
			[DataMember(Name = "name")]
			public string Name { get; set; }

			[DataMember(Name = "userName")]
			public string UserName { get; set; }

			[Required]
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
