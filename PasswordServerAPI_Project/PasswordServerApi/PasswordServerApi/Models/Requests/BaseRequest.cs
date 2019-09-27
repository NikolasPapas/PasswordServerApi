using System;
using System.Runtime.Serialization;

namespace PasswordServerApi.Models.Requests
{
	[DataContract]
	public class BaseRequest
	{
		[DataMember(Name = "actionId")]
		public Guid ActionId { get; set; }

		[DataMember(Name = "role")]
		public string Role { get; set; }

	}
}
