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
	/*
	 * https://localhost:44390/api/Authentication/logIn
	 * {
	 * "username":"npapazian105",
	 * "password":"123105"
	 * }
	*/

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

		/*
		 * https://localhost:44390/api/passwords
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns>Return List With All Passwords</returns>
		[HttpPost]
		[ActionName("getPasswords")]
		public Response<IEnumerable<PasswordDto>> Get()
		{
			return new Response<IEnumerable<PasswordDto>>()
			{
				Payload = _passwordService.GetPasswords(),
				Warnnings = new List<string>()
			};
		}
		 */

		/// <summary>
		/// 
		/// </summary>
		/// <param name=""></param>
		/// <returns></returns>
		[HttpPost("passwordAction")]
		public Response<List<PasswordDto>> PasswordAction([FromBody] PasswordActionRequest request)
		{
			request.AccountId = Guid.Parse(HttpContext.User.Identity.Name);
			return _passwordService.PasswordAction(request);
		}
	}
}
