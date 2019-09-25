using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PasswordServerApi.DTO;
using PasswordServerApi.Interfaces;
using PasswordServerApi.Models.Enums;

namespace PasswordServerApi.Controllers
{
	//api/passwords
	[Route("api/[controller]")]
    [ApiController]
    public class PasswordsController : ControllerBase
	{
		private readonly IPasswordService _passwordService;

		public PasswordsController(IPasswordService passwordService)
		{
			_passwordService = passwordService;
		}

		// GET: api/Passwords
		[HttpGet]
		[ActionName("getPasswords")]
		public IEnumerable<PasswordDto> Get()
        {
			return _passwordService.GetPasswords();
        }

        // GET: api/Passwords/5
        [HttpGet("{id}", Name = "GetPasswords")]
        public string Get(int id)
        {
            return "GetPasswords";
        }

        // POST: api/Passwords
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Passwords/5
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
