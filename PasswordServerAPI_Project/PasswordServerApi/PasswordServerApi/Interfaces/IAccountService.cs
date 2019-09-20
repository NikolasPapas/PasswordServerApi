﻿using PasswordServerApi.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.Interfaces
{
	interface IAccountService
	{
		IEnumerable<AccountDto> GetAccounts();

		AccountDto GetAccount();
	}
}
