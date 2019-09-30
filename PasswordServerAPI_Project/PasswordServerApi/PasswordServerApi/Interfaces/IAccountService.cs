using PasswordServerApi.DTO;
using PasswordServerApi.Models.Account.Requests;
using PasswordServerApi.Models.Requests;
using PasswordServerApi.Models.Responces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.Interfaces
{
	public interface IAccountService
	{
		Response<List<AccountDto>> ExecuteAction(AccountActionRequest request);


	}
}
