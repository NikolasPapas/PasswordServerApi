

using PasswordServerApi.Security.SecurityModels;
using System;

namespace PasswordServerApi.Security
{
	public interface IAuthenticateService
	{
		bool IsAuthenticated(TokenRequest request, out string token);

		bool IsAuthorized(Guid id, string Token);
	}
}
