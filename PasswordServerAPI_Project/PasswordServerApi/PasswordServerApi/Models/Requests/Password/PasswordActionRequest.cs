using PasswordServerApi.DTO;
using System.Runtime.Serialization;

namespace PasswordServerApi.Models.Requests.Password
{
	[DataContract]
	public class PasswordActionRequest : BaseRequest
	{
		[DataMember(Name = "password")]
		public PasswordDto Password { get; set; }
	}
}
