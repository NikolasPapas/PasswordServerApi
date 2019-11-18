using System;
using System.Runtime.Serialization;

namespace PasswordServerApi.Models.API
{
	[DataContract]
	public class FromSite
	{
		[DataMember(Name = "site")]
		public string Site { get; set; }

		[DataMember(Name = "lastDate")]
		public DateTime? LastDate { get; set; }

	}
}
