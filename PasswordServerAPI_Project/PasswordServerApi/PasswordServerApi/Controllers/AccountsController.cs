using System;
using System.Collections.Generic;
using System.Linq;
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
	/*
	 * https://localhost:44390/api/Authentication/logIn
	 * {
	 * "username":"npapazian105",
	 * "password":"123105"
	 * }
	*/

	//api/accounts
	[Authorize(Roles = Role.Admin + " , " + Role.Viewer)]
	[Route("api/[controller]")]
	[ApiController]
	public class AccountsController : ControllerBase
	{
		private readonly IAccountService _accountService;

		public AccountsController(IAccountService accountService)
		{
			_accountService = accountService;
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


		/*
		 * https://localhost:44390/api/accounts/accountAction
		 *	 {
 		 *		"ActionId":"1086495e-fd61-4397-b3a9-87b737adeddd",
 		 *		"Role":"Admin",
		 *		"Account":
		 *			{
		 *				"accountId": "2a645ff3-79ba-48ed-9725-2d09189b64d9",
		 *				"firstName": "nikolas105",
		 *				"lastName": "papazian105",
		 *				"userName": "npapazian105",
		 *				"email": "npapazian105@cite.gr",
		 *				"sex": 0,
		 *				"lastLogIn": "2019-09-27T12:19:47.141142+03:00",
		 *				"password": "123105",
		 *				"role": "Admin",
		 *				"curentTocken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiMmE2NDVmZjMtNzliYS00OGVkLTk3MjUtMmQwOTE4OWI2NGQ5IiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE1Njk1Nzk2ODIsImlzcyI6ImxvY2FsaG9zdCIsImF1ZCI6IlNhbXBsZUF1ZGllbmNlIn0.uWPDk1zzf5Ir32_S9hveJjkue_Db6zRU_Jt9b_wqeDc",
		 *				"passwords": []
		 *			}
		 *	 }
		*/
		/// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost("accountAction")]
		public Response<AccountDto> AccountAction([FromBody] AccountActionRequest request)
		{
			request.AccountId = Guid.Parse(HttpContext.User.Identity.Name);
			return _accountService.ExecuteAction(request);
		}

	}
}