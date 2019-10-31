using PasswordServerApi.DTO;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PasswordServerApi.Models.Responces
{
	[DataContract]
	public class PasswordActionResponse : BaseResponse
	{
		[DataMember(Name = "passwords")]
		public List<PasswordDto> Passwords { get; set; }
	}
}