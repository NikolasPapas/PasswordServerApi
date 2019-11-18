﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace PasswordServerApi.Databases.DataModels
{
	[DataContract]
	public class LoginTokenModel
	{
		[Required]
		[DataMember(Name = "LoginTokenId")]
		public string LoginTokenId { get; set; }

		[Required]
		[DataMember(Name = "userId")]
		public string UserId { get; set; }

		[Required]
		[DataMember(Name = "token")]
		public string Token { get; set; }

		[Required]
		[DataMember(Name = "userAgent")]
		public string UserAgent { get; set; }

		[Required]
		[DataMember(Name = "lastLogIn")]
		public DateTime? LastLogIn { get; set; }

	}
}