using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PasswordServerApi.DTO;
using PasswordServerApi.Interfaces;
using PasswordServerApi.Models.Enums;
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
        /// <returns>Return List With All Passwords</returns>
		[HttpGet]
        [ActionName("getPasswords")]
		public IEnumerable<PasswordDto> Get()
		{
			return _passwordService.GetPasswords();
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>return Passworn With Guid</returns>
		[HttpPost("getById")]
        public PasswordDto GetById([FromBody] string id)
		{
            return _passwordService.GetPassword(Guid.Parse(id)); 
		}
	}
}
