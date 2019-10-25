using PasswordServerApi.DTO;
using PasswordServerApi.Interfaces;
using PasswordServerApi.Models.Account.Requests;
using PasswordServerApi.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.Security
{
	public class UserManagementService : IUserManagementService
	{

		IBaseService _baseService;
		public UserManagementService(IBaseService baseService)
		{
			_baseService = baseService;
		}

		public AccountDto FindValidUser(string userName, string password)
		{
			return _baseService.GetSpesificAccount(new AccountActionRequest()
			{
				Account = new AccountDto()
				{
					UserName = userName,
					Password = password
				}
			});
		}

		public void SaveNewToken(Guid id, string Token)
		{
			var account = FindValidUserID(id);
			if (account == null) throw new Exception("No User");
			account.CurrentToken = Token;
			account.LastLogIn = DateTime.Now;

			_baseService.UpdateAccount(account,account.Role, true);
		}

		public AccountDto FindValidUserID(Guid UserId)
		{
			return _baseService.GetAccountById(UserId,true);
		}
	}
}
