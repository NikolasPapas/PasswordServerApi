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
		private readonly ILoggingService _logger;

		public PasswordsController(ILoggingService logger, IPasswordService passwordService)
		{
			_passwordService = passwordService;
			_logger = logger;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost("passwordAction")]
		public Response<PasswordActionResponse> PasswordAction([FromBody] PasswordActionRequest request)
		{
			try
			{
				return _passwordService.PasswordAction(request, Guid.Parse(HttpContext.User.Identity.Name));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}
			return new Response<PasswordActionResponse>() { Error = "Error On Runn" };
		}
	}
}
