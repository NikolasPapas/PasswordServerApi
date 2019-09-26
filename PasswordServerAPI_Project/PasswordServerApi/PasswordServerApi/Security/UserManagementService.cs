using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.Security
{
	public class UserManagementService : IUserManagementService
	{
		public bool IsValidUser(string userName, string password)
		{
			if (userName == "123" && password == "123")
				return true;
			return false;
		}
	}
}
