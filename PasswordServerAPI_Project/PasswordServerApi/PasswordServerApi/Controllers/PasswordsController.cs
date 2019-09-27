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

		public PasswordsController(IPasswordService passwordService)
		{
			_passwordService = passwordService;
		}

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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns>return Passworn With Guid</returns>
		[HttpPost("getById")]
		public Response<PasswordDto> GetById([FromBody] string id)
		{
			return new Response<PasswordDto>()
			{
				Payload = _passwordService.GetPassword(Guid.Parse(id)),
				Warnnings = new List<string>()
			};
		}
	}
}
