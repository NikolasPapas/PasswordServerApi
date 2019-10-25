using PasswordServerApi.DTO;
using PasswordServerApi.Models.Requests.Password;
using PasswordServerApi.Models.Responces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.Interfaces
{
	public interface IPasswordService
	{
		Response<PasswordActionResponse> PasswordAction(PasswordActionRequest request, Guid userID);
	}
}
