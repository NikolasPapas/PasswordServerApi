using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PasswordServerApi.Models.DTO
{
	[DataContract]
	public class LoginTokenDto
	{
		[DataMember(Name = "loginTokenId")]
		public Guid LoginTokenId { get; set; }

		[DataMember(Name = "userId")]
		public Guid UserId { get; set; }

		[DataMember(Name = "token")]
		public string Token { get; set; }

		[DataMember(Name = "userAgent")]
		public string UserAgent { get; set; }

		[DataMember(Name = "lastLogIn")]
		public DateTime? LastLogIn { get; set; }

	}
}
