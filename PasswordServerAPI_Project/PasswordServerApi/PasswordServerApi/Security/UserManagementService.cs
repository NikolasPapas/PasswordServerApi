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

			return _baseService.GetAccounts(new SearchAccountsRequest()
			{
				UserName = userName,
				Password = password
			}).FirstOrDefault();
		}

		public void SaveNewToken(Guid id, string Token)
		{
			var account = FindValidUserID(id);
			if (account == null) throw new Exception("No User");
			account.CurentToken = Token;
			account.LastLogIn = DateTime.Now;

			_baseService.UpdateAccount(account,true);
		}

		public AccountDto FindValidUserID(Guid UserId)
		{
			return _baseService.GetAccountById(UserId);
		}
	}
}
