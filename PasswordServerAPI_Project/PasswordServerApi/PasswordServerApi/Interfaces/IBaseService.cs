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
		AccountDto GetSpesificAccount(AccountActionRequest request);

		IEnumerable<AccountDto> GetAccounts(AccountActionRequest request);

		AccountDto UpdateAccount(AccountDto accountDto, bool full);

		AccountDto GetAccountById(Guid id);

		AccountDto AddNewAccount(AccountDto request);



		PasswordDto GetPassword(Guid id);

		PasswordDto UpdatePassword(PasswordDto passwordDto);

		IEnumerable<PasswordDto> GetPasswords(PasswordActionRequest request);

	}
}
