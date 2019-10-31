using PasswordServerApi.DTO;
using PasswordServerApi.Models.Account.Requests;
using PasswordServerApi.Models.Requests.Password;
using System;
using System.Collections.Generic;

namespace PasswordServerApi.Interfaces
{
	public interface IBaseService
	{
		AccountDto GetSpesificAccount(AccountActionRequest request);

		IEnumerable<AccountDto> GetAccounts(AccountActionRequest request,bool full);

		AccountDto UpdateAccount(AccountDto accountDto, string Role, bool full);

		AccountDto GetAccountById(Guid id, bool full);

		AccountDto AddNewAccount(AccountDto request);

		AccountDto RemoveAccount(AccountDto request);



		PasswordDto GetPassword(Guid id);

		PasswordDto UpdatePassword(PasswordDto passwordDto);

		IEnumerable<PasswordDto> GetPasswords(PasswordActionRequest request, AccountDto account);

		PasswordDto AddNewPassword(PasswordDto requestPassword);

		PasswordDto RemovePassword(PasswordDto requestPassword);

		void FilldDatabase(List<AccountDto> accounts);
	}
}
