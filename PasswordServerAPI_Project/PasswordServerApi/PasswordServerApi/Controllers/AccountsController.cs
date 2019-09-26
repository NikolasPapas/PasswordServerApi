using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PasswordServerApi.DTO;
using PasswordServerApi.Interfaces;
using PasswordServerApi.Security.SecurityModels;

namespace PasswordServerApi.Controllers
{
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Return A List With All Accounts</returns>
		[HttpGet]
        public IEnumerable<AccountDto> Get()
        {
            return _accountService.GetAccounts();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return A Account From Guid</returns>
        [HttpPost("getById")]
        public AccountDto GetById([FromBody] string id)
        {
            return _accountService.GetAccount(Guid.Parse(id));
        }

    }
}
