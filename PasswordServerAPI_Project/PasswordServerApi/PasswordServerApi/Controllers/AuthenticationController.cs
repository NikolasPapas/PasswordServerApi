using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PasswordServerApi.DTO;
using PasswordServerApi.Interfaces;
using PasswordServerApi.Models;
using PasswordServerApi.Models.Responces;
using PasswordServerApi.Security.SecurityModels;
using PasswordServerApi.Service;
using System;
using System.Collections.Generic;
using System.Linq;

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


        [AllowAnonymous]
        [HttpPost("logIn")]
        public Response<ResponceTokenRequest> RequestToken([FromBody] TokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return null;
            }

            string token;
            string userAgent = Request.Headers["User-Agent"].ToString();
            List<ApplicationAction> actionsForUser = _authService.IsAuthenticated(request, userAgent, out token);



            if (actionsForUser != null)
            {
                return new Response<ResponceTokenRequest>()
                {
                    Payload = new ResponceTokenRequest() { Token = token, Actions = actionsForUser },
                    Warnnings = new List<string>() { }
                };
            }
            return null;
        }
    }
}
