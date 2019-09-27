using PasswordServerApi.DTO;
using System.Runtime.Serialization;

namespace PasswordServerApi.Models.Requests.Password
{
	[DataContract]
	public class PasswordActionRequest : SearchPasswordsRequest
	{
		[DataMember(Name = "password")]
		public PasswordDto Password { get; set; }

	}
}
