using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PasswordServerApi.DTO;
using PasswordServerApi.Interfaces;

namespace PasswordServerApi.Service
{
	public class AccountService : IAccountService
	{
		public AccountService()
		{

		}

		public IEnumerable<AccountDto> GetAccounts()
		{


			return new List<AccountDto>() { };
		}


		public AccountDto GetAccount()
		{
			return new AccountDto() { };
		}

	}
}
