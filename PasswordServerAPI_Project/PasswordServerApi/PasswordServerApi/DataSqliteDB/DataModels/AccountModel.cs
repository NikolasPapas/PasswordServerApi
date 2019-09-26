using PasswordServerApi.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PasswordServerApi.DataSqliteDB
{
	[DataContract]
	public class AccountModel
	{
		[Required]
		[DataMember(Name = "accountId")]
		public string AccountId { get; set; }

		[Required]
		[DataMember(Name = "firstName")]
		public string FirstName { get; set; }

		[Required]
		[DataMember(Name = "lastName")]
		public string LastName { get; set; }

		[Required]
		[DataMember(Name = "userName")]
		public string UserName { get; set; }

		[Required]
		[DataMember(Name = "email")]
		public string Email { get; set; }

		[DataMember(Name = "sex")]
		public Sex Sex { get; set; }

		[Required]
		[DataMember(Name = "lastLogIn")]
		public DateTime? LastLogIn { get; set; }


		//PRIVATE
		[Required]
		[DataMember(Name = "password")]
		public string Password { get; set; }

		[Required]
		[DataMember(Name = "role")]
		public string Role { get; set; }

		[Required]
		[DataMember(Name = "curentTocke")]
		public string CurentToken { get; set; }


		[DataMember(Name = "passwords")]
		public List<string> PasswordIds { get; set; }

	}
}
