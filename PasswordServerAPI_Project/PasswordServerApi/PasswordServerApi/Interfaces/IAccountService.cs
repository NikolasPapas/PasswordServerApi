using PasswordServerApi.DTO;
using PasswordServerApi.Models.Account.Requests;
using PasswordServerApi.Models.DTO;
using PasswordServerApi.Models.Requests;
using PasswordServerApi.Models.Responces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.Interfaces
{
	public interface IAccountService
	{
		AccountActionResponse ExecuteAction(AccountActionRequest request, Guid userID);

		AccountActionResponse GetMyAccount(BaseRequest request, Guid userID);

		List<LoginTokenDto> TokenActions(TokenActionRequest tokenActionRequest, Guid userID);

	}
}
