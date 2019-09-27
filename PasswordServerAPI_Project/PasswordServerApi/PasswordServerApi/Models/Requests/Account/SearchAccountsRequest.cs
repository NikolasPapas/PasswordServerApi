using PasswordServerApi.Models.Requests;
using System.Runtime.Serialization;

namespace PasswordServerApi.Models.Account.Requests
{
	[DataContract]
	public class SearchAccountsRequest : BaseRequest
	{
		[DataMember(Name = "userName")]
		public string UserName { get; set; }

		[DataMember(Name = "password")]
		public string Password { get; set; }

		[DataMember(Name = "email")]
		public string Email { get; set; }

		[DataMember(Name = "customFilterID")]
		public string CustomFilterID { get; set; }

		[DataMember(Name = "customFilterValue")]
		public object CustomFilterValue { get; set; }

	}
}
