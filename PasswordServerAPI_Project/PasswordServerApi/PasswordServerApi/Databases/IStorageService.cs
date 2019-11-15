using PasswordServerApi.DTO;
using PasswordServerApi.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.StorageLayer
{
	public interface IStorageService
	{
		#region Account

		List<AccountDto> GetAccounts();

		AccountDto SetAccount(AccountDto addAccount);

		void DeleteAccount(AccountDto addAccount);

		#endregion

		#region Password

		List<PasswordDto> GetPasswords();

		PasswordDto SetPassword(PasswordDto addPassword);

		void DeletePassword(PasswordDto addPassword);

		#endregion

		#region Tokens

		List<LoginTokenDto> GetTokens();

		List<LoginTokenDto> SetToken(LoginTokenDto loginToken);

		void DeleteToken(LoginTokenDto loginToken);

		#endregion
	}
}
