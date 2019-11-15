using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PasswordServerApi.DTO;
using PasswordServerApi.Interfaces;
using PasswordServerApi.Models.Account.Requests;
using PasswordServerApi.Models.DTO;
using PasswordServerApi.Models.Requests;
using PasswordServerApi.Models.Requests.Account;
using PasswordServerApi.Models.Requests.Password;
using PasswordServerApi.Models.Responces;
using PasswordServerApi.Security.SecurityModels;

namespace PasswordServerApi.Controllers
{
	//api/accounts

	[Route("api/[controller]")]
	[ApiController]
	public class AccountsController : ControllerBase
	{
		private readonly IAccountService _accountService;
		private readonly IPasswordService _passwordService;
		private readonly IExportService _exportService;
		private readonly ILoggingService _logger;
		private readonly IExceptionHandler _exceptionHandler;

		public AccountsController(IAccountService accountService, IPasswordService passwordService, ILoggingService logger,  IExportService exportService, IExceptionHandler exceptionHandler)
		{
			_accountService = accountService;
			_passwordService = passwordService;
			_exportService = exportService;
			_logger = logger;
			_exceptionHandler = exceptionHandler;
		}

		[Authorize]
		[HttpPost("getMyAccount")]
		public Response<AccountActionResponse> GetMyAccount([FromBody] BaseRequest request)
		{

			return _exceptionHandler.HandleException(() => _accountService.GetMyAccount(request, Guid.Parse(HttpContext.User.Identity.Name)), request.ActionId);
		}

		[Authorize]
		[HttpPost("tokenActions")]
		public Response<List<LoginTokenDto>> TokenActions([FromBody] TokenActionRequest request)
		{
			return _exceptionHandler.HandleException(() => _accountService.TokenActions(request, Guid.Parse(HttpContext.User.Identity.Name)), Guid.Parse(HttpContext.User.Identity.Name));
		}


		[Authorize(Roles = Role.Admin)]
		[HttpPost("accountAction")]
		public Response<AccountActionResponse> AccountAction([FromBody] AccountActionRequest request)
		{

			return _exceptionHandler.HandleException(() => _accountService.ExecuteAction(request, Guid.Parse(HttpContext.User.Identity.Name)), request.ActionId);
		}

		[Authorize(Roles = Role.User + " , " + Role.Admin)]
		[HttpPost("passwordAction")]
		public Response<PasswordActionResponse> PasswordAction([FromBody] PasswordActionRequest request)
		{
			return _exceptionHandler.HandleException(() => _passwordService.PasswordAction(request, Guid.Parse(HttpContext.User.Identity.Name)), request.ActionId);
		}

		[Authorize(Roles = Role.Admin)]
		[HttpPost("exportReport")]
		public HttpResponseMessage ExportReport([FromBody] BaseRequest request)
		{
			try
			{
				return _exportService.Export();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}
			return null;
		}

		[Authorize(Roles = Role.Admin)]
		[HttpPost("importData")]
		public StoreDocumentResponse ImportData([FromBody] StoreDocumentRequest request)
		{
			try
			{
				return _exportService.Import(request);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}
			return null;
		}

	}
}