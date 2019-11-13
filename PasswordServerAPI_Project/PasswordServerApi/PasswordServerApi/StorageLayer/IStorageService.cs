using PasswordServerApi.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.StorageLayer
{
	public interface IStorageService
	{

		List<AccountDto> GetAccountsDto();

		AccountDto SetAccountsDto(AccountDto addAccount);

		void DeleteAccountsDto(AccountDto addAccount);

		List<PasswordDto> GetPasswordsDto();

		PasswordDto SetPasswordsDto(PasswordDto addPassword);

		void DeletePasswordsDto(PasswordDto addPassword);


	}
}
