using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace PasswordServerApi.Databases.DataModels
{
	[DataContract]
	public class LoginTokenModel
	{
		[Key]
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

		[DataMember(Name = "lastLogIn")]
		public DateTime? LastLogIn { get; set; }

	}
}
