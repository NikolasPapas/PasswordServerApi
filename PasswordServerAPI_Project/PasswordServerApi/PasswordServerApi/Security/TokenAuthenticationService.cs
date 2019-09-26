using PasswordServerApi.Security.SecurityModels;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace PasswordServerApi.Security
{
	public class TokenAuthenticationService : IAuthenticateService
	{
		private readonly IUserManagementService _userManagementService;
		private readonly TokenManagement _tokenManagement;

		public TokenAuthenticationService(IUserManagementService service, Microsoft.Extensions.Options.IOptions<TokenManagement> tokenManagement)
		{
			_userManagementService = service;
			_tokenManagement = tokenManagement.Value;
		}

		public bool IsAuthenticated(TokenRequest request, out string token)
		{

			token = string.Empty;
			if (!_userManagementService.IsValidUser(request.Username, request.Password)) return false;

			var claim = new[]
			{
				new Claim(ClaimTypes.Name, request.Username)
			};
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var jwtToken = new JwtSecurityToken(
				_tokenManagement.Issuer,
				_tokenManagement.Audience,
				claim,
				expires: DateTime.Now.AddMinutes(3),
				signingCredentials: credentials
			);
			token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
			return true;

		}


		public bool IsAuthorized(string id)
		{
			if (id == "1")
				return true;
			return false;
		}
	}
}

