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
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordsController : ControllerBase
	{
		private readonly IAccountService _accountService;

		public PasswordsController(IAccountService accountService)
		{
			_accountService = accountService;
		}

		// GET: api/Passwords
		[HttpGet]
		[ActionName("getPasswords")]
		public IEnumerable<AccountDto> Get()
        {
			var account = new AccountDto()
			{
				AccountId = Guid.NewGuid(),
				FirstName = "nikolas",
				LastName = "papazian",
				UserName = "npapazian",
				Email = "npapazian@cite.Gr",
				Password = "123",
				Sex = Sex.Male,
				LastLogIn = null,
				Passwords =new List<PasswordDto>() { },
			};

			var pass = new PasswordDto()
			{
				PasswordId = Guid.NewGuid(),
				Name="Google",
				UserName="nikolaspapazian@gmail.com",
				Password="123",
				LogInLink="google.com",
				Sensitivity = Sensitivity.OnlyUser,
				Strength=Strength.VeryWeak
			};
			account.Passwords.Add(pass);

			return new AccountDto[] { account };
        }

        // GET: api/Passwords/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
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
