﻿using System;
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

		/*
		 * https://localhost:44390/api/accounts/
		 *	{
		 *		"ActionId":"c25b9787-8751-4fbd-bc6c-c63a48026d30",
		 *		"UserName":"npapazian105",
		 *		//"Password":"123105",
		 *		//"Email":"npapazian105@cite.gr",
		 *	}
		/// <summary>
		/// 
		/// </summary>
		/// <returns>Return A List With All Accounts</returns>
		[HttpPost]
		public Response<IEnumerable<AccountDto>> Get([FromBody] SearchAccountsRequest request)
		{
			return new Response<IEnumerable<AccountDto>>()
			{
				Payload = _accountService.GetAccounts(request),
				Warnnings = new List<string>()
			};
		}
		*/

		/// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost("accountAction")]
		public Response<List<AccountDto>> AccountAction([FromBody] AccountActionRequest request)
		{
			try
			{
				request.AccountId = Guid.Parse(HttpContext.User.Identity.Name);
				return _accountService.ExecuteAction(request);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}
			return new Response<List<AccountDto>>() { Error = "Error On Runn" };
		}



		[HttpPost("exportReport")]
		public HttpResponseMessage ExportReport()
		{
			try
			{
				return _exportService.Export();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}
		}

	}
}