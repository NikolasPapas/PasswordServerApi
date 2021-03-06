﻿using System;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using PasswordServerApi.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PasswordServerApi.DTO
{
	[DataContract]
	public class AccountDto
	{
		[DataMember(Name = "accountId")]
		public Guid AccountId { get; set; }

		[DataMember(Name = "firstName")]
		public string FirstName { get; set; }

		[DataMember(Name = "lastName")]
		public string LastName { get; set; }

		[DataMember(Name = "userName")]
		public string UserName { get; set; }

		[DataMember(Name = "email")]
		public string Email { get; set; }

		[DataMember(Name = "sex")]
		public Sex Sex { get; set; }

		[DataMember(Name = "lastLogIn")]
		public DateTime? LastLogIn { get; set; }


		//PRIVATE
		[DataMember(Name = "password")]
		public string Password { get; set; }

		[DataMember(Name = "role")]
		public string Role { get; set; }

		//[DataMember(Name = "currentToken")]
		//public string CurrentToken { get; set; }

		[DataMember(Name = "passwords")]
		public List<PasswordDto> Passwords { get; set; }
	}
}
