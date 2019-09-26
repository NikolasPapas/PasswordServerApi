using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordServerApi.Security.SecurityModels;

namespace PasswordServerApi.Security
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

		private readonly IAuthenticateService _authService;
		public AuthenticationController(IAuthenticateService authService)
		{
			_authService = authService;
		}

		//http://localhost:53257/api/Authentication/logIn
		//https://localhost:44390/api/Authentication/logIn
		[AllowAnonymous]
		[HttpPost("logIn")]
		public ActionResult RequestToken([FromBody] TokenRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			string token;
			if (_authService.IsAuthenticated(request, out token))
			{
				return Ok(token);
			}
			return BadRequest("Invalid Request");
		}
    }
}
