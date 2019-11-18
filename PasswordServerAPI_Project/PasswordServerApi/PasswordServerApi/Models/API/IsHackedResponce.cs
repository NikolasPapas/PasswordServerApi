using System.Runtime.Serialization;

namespace PasswordServerApi.Models.API
{
	[DataContract]
	public class IsHackedResponce
	{
		[DataMember(Name = "email")]
		public string Email { get; set; }

		[DataMember(Name = "isHacked")]
		public bool IsHacked { get; set; }

	}
}
