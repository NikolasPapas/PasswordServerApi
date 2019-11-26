using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PassswordHackScanner.Models
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
