using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PasswordServerApi.DTO;
using PasswordServerApi.Interfaces;
using PasswordServerApi.Models.Enums;

namespace PasswordServerApi.Service
{
	public class AccountService : IAccountService
	{
		IBaseService _baseService;

		public AccountService( IBaseService baseService)
		{
			_baseService = baseService;
		}

		public IEnumerable<AccountDto> GetAccounts()
		{
			return new List<AccountDto>() { _baseService.GetDumyAccount(1), _baseService.GetDumyAccount(2), _baseService.GetDumyAccount(2), _baseService.GetDumyAccount(3), _baseService.GetDumyAccount(4), _baseService.GetDumyAccount(5), _baseService.GetDumyAccount(105) };
		}


		public AccountDto GetAccount()
		{
			return _baseService.GetDumyAccount(1);
		}
	}
}
