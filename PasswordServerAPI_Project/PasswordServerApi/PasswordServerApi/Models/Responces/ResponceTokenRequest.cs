using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PasswordServerApi.Models.Responces
{
	[DataContract]
	public class ResponceTokenRequest
	{
		[DataMember(Name = "token")]
		public string Token { get; set; }

		[DataMember(Name = "actions")]
		public List<ApplicationAction> Actions { get; set; }
	}
}
