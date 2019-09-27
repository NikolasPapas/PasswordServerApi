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
		Response<AccountDto> ExecuteAction(AccountActionRequest request);


		IEnumerable<AccountDto> GetAccounts(SearchAccountsRequest request);

		AccountDto UpdateAccount(AccountDto account);

        AccountDto GetAccountById(Guid id);

    }
}
