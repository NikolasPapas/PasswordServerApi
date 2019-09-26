using PasswordServerApi.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.Interfaces
{
	public interface IAccountService
	{
		IEnumerable<AccountDto> GetAccounts();

		AccountDto UpdateAccount(AccountDto account);

		AccountDto GetAccount(Guid id);
	}
}
