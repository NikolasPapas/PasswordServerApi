using PasswordServerApi.Models.DTO;
using PasswordServerApi.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PasswordServerApi.Models.Requests
{
	[DataContract]
	public class TokenActionRequest
	{
		[DataMember(Name = "token")]
		public LoginTokenDto Token { get; set; }

		[DataMember(Name = "action")]
		public TokenRequestActionEnum Action { get; set; }
	}
}
