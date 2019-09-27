using PasswordServerApi.DTO;
using PasswordServerApi.Models.Account.Requests;
using PasswordServerApi.Models.Requests.Password;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.Interfaces
{
    public interface IBaseService
	{
		IEnumerable<AccountDto> GetAccounts(SearchAccountsRequest request);

		AccountDto UpdateAccount(AccountDto accountDto, bool full);

		AccountDto GetAccountById(Guid id);


		PasswordDto GetPassword(Guid id);

		PasswordDto UpdatePassword(PasswordDto passwordDto);

		IEnumerable<PasswordDto> GetPasswords(PasswordActionRequest request);

	}
}
