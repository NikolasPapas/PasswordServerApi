using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PasswordServerApi.DTO;
using PasswordServerApi.Interfaces;
using PasswordServerApi.Models.Account.Requests;
using PasswordServerApi.Models.Requests;
using PasswordServerApi.Models.Requests.Account;
using PasswordServerApi.Models.Responces;
using PasswordServerApi.Security.SecurityModels;

namespace PasswordServerApi.Controllers
{
	//api/accounts
	[Authorize(Roles = Role.Admin)]
	[Route("api/[controller]")]
	[ApiController]
	public class AccountsController : ControllerBase
	{
		private readonly IAccountService _accountService;
		private readonly IExportService _exportService;
		private readonly ILoggingService _logger;

		public AccountsController(ILoggingService logger, IAccountService accountService, IExportService exportService)
		{
			_accountService = accountService;
			_exportService = exportService;
			_logger = logger;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost("accountAction")]
		public Response<AccountActionResponse> AccountAction([FromBody] AccountActionRequest request)
		{
			try
			{
				return _accountService.ExecuteAction(request, Guid.Parse(HttpContext.User.Identity.Name));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}
			return new Response<AccountActionResponse>() { Error = "Error On Runn" };
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
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