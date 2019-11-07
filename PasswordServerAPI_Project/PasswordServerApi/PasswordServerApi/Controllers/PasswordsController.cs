using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
		private readonly IExceptionHandler _exceptionHandler;

		public PasswordsController(ILoggingService logger, IPasswordService passwordService, IExceptionHandler exceptionHandler)
		{
			_passwordService = passwordService;
			_logger = logger;
			_exceptionHandler = exceptionHandler;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost("passwordAction")]
		public Response<PasswordActionResponse> PasswordAction([FromBody] PasswordActionRequest request)
		{
			return _exceptionHandler.HandleException(() => _passwordService.PasswordAction(request, Guid.Parse(HttpContext.User.Identity.Name)), request.ActionId);
			//try
			//{
			//	return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(_passwordService.PasswordAction(request, Guid.Parse(HttpContext.User.Identity.Name)))) };
			//}
			//catch (Exception ex)
			//{
			//	_logger.LogError(ex.Message);
			//}
			//return new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(new Response() { Error = "Error On Runn" })) };

		}
	}
}
