using PasswordServerApi.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PasswordServerApi.Models.Responces
{
	[DataContract]
	public class AccountActionResponse : BaseResponse
	{
		[DataMember(Name = "accounts")]
		public List<AccountDto> Accounts { get; set; }
	}

}
