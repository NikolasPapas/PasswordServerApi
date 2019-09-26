using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PasswordServerApi.DTO;
using PasswordServerApi.Interfaces;

namespace PasswordServerApi.Controllers
{
	//api/accounts
	[Authorize]
	[Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
		private readonly IAccountService _accountService;

		public AccountsController(IAccountService accountService)
		{
			_accountService = accountService;
		}

		/// <summary>
		/// Get Accounts List
		/// </summary>
		/// <returns>200</returns>
		// GET: api/Accounts
		[HttpGet]
        public IEnumerable<AccountDto> Get()
        {
			return _accountService.GetAccounts();
        }

        // GET: api/Accounts/5
        [HttpGet("{id}", Name = "GetAccounts")]
        public string Get(int id)
        {
            return "GetAccounts";
        }

        // POST: api/Accounts
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Accounts/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
