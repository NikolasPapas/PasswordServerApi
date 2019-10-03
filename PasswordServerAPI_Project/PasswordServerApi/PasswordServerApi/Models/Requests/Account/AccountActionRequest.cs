using PasswordServerApi.DTO;
using PasswordServerApi.Models.Requests;
using System.Runtime.Serialization;

namespace PasswordServerApi.Models.Account.Requests
{
	[DataContract]
	public class AccountActionRequest : BaseRequest
	{
		[DataMember(Name = "account")]
		public AccountDto Account { get; set; }
	}
}
