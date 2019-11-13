using PasswordServerApi.DataSqliteDB.DataModels;
using PasswordServerApi.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.StorageLayer
{
	public class StorageServiceFile : IStorageService
	{
		public StorageServiceFile()
		{

		}


		#region Account

		public List<AccountDto> GetAccountsDto()
		{
			return new List<AccountDto>();
		}

		public AccountDto SetAccountsDto(AccountDto addAccount)
		{
			return new AccountDto();
		}

		public void DeleteAccountsDto(AccountDto addAccount)
		{

		}

		#endregion

		#region Password

		public List<PasswordDto>  GetPasswordsDto()
		{
			return new List<PasswordDto>();
		}

		public PasswordDto SetPasswordsDto(PasswordDto addPassword)
		{
			return new PasswordDto();
		}

		public void DeletePasswordsDto(PasswordDto addPassword)
		{

		}

		#endregion
	}
}
