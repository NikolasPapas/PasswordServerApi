using System.Collections.Generic;
using System.Runtime.Serialization;


namespace PasswordServerApi.Models.Responces
{
	[DataContract]
	public class BaseResponse
	{
		[DataMember(Name = "warningMessages")]
		public List<string> WarningMessages { get; set; }
	}
}
