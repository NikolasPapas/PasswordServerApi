using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PasswordServerApi.Models.API
{
	[DataContract]
	public class HackTimeResponce
	{
		[DataMember(Name = "email")]
		public string Email { get; set; }

		[DataMember(Name = "isHacked")]
		public bool IsHacked { get; set; }

		[DataMember(Name = "fromSites")]
		public List<FromSite> FromSites { get; set; }
	}
}
