using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordServerApi.DTO;
using PasswordServerApi.Interfaces;
using PasswordServerApi.Models.Requests.Password;
using PasswordServerApi.Models.Responces;
using PasswordServerApi.Security.SecurityModels;

namespace PasswordServerApi.Controllers
{
	//api/passwords
	[Authorize(Roles = Role.User + " , " + Role.Admin)]
	[Route("api/[controller]")]
	[ApiController]
	public class PasswordsController : ControllerBase
	{
		private readonly IPasswordService _passwordService;

		public PasswordsController(IPasswordService passwordService)
		{
			_passwordService = passwordService;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost("passwordAction")]
		public Response<List<PasswordDto>> PasswordAction([FromBody] PasswordActionRequest request)
		{
			request.AccountId = Guid.Parse(HttpContext.User.Identity.Name);
			return _passwordService.PasswordAction(request);
		}
	}
}
