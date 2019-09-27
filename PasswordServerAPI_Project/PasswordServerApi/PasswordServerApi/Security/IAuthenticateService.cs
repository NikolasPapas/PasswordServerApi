

using PasswordServerApi.Models;
using PasswordServerApi.Security.SecurityModels;
using System;
using System.Collections.Generic;

namespace PasswordServerApi.Security
{
	public interface IAuthenticateService
	{
		List<ApplicationAction> IsAuthenticated(TokenRequest request, out string token);

		bool IsAuthorized(Guid id, string Token);
	}
}
