using PasswordServerApi.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.Security
{
	public interface IUserManagementService
	{
		AccountDto FindValidUserID(Guid UserId);

		void SaveNewToken(Guid id, string userAgent, string Token);

		AccountDto FindValidUser(string userName, string password);

	}
}
