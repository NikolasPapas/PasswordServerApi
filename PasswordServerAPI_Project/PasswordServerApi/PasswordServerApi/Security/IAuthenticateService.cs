

using PasswordServerApi.Security.SecurityModels;

namespace PasswordServerApi.Security
{
	public interface IAuthenticateService
	{
		bool IsAuthenticated(TokenRequest request, out string token);

		bool IsAuthorized(string id);
	}
}
