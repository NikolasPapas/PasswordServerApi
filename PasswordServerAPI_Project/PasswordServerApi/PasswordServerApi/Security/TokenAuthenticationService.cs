using PasswordServerApi.Security.SecurityModels;
using System;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using PasswordServerApi.DTO;
using PasswordServerApi.Models;
using System.Collections.Generic;
using PasswordServerApi.Service;
using PasswordServerApi.Interfaces;
using PasswordServerApi.Models.DTO;

namespace PasswordServerApi.Security
{
	public class TokenAuthenticationService : IAuthenticateService
	{
		private readonly IUserManagementService _userManagementService;
		private readonly TokenManagement _tokenManagement;
		private readonly ILoggingService _logger;
		private readonly IBaseService _baseService;


		public TokenAuthenticationService(IUserManagementService service, Microsoft.Extensions.Options.IOptions<TokenManagement> tokenManagement, IBaseService baseService, ILoggingService logger)
		{
			_userManagementService = service;
			_tokenManagement = tokenManagement.Value;
			_baseService = baseService;
			_logger = logger;
		}

		public List<ApplicationAction> IsAuthenticated(TokenRequest request, string userAgent, out string token)
		{
			List<ApplicationAction> ActionList = new List<ApplicationAction>();
			token = string.Empty;
			AccountDto user = _userManagementService.FindValidUser(request.Username, request.Password);
			if (user == null)
			{
				_logger.LogInfo($"UnAuthorized UserName: ${request.Username}");
				return null;
			}

			var claim = new[]
			{
				new Claim(ClaimTypes.Name, user.AccountId.ToString()),
				new Claim(ClaimTypes.Role, user.Role)
			};
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var jwtToken = new JwtSecurityToken(
				_tokenManagement.Issuer,
				_tokenManagement.Audience,
				claim,
				expires: DateTime.Now.AddMinutes(10),
				signingCredentials: credentials
			);
			token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

			_logger.LogInfo($"User ${user.UserName} is logged in");

			_userManagementService.SaveNewToken(user.AccountId, userAgent, token);
			return StaticConfiguration.GetAcrionByProfile(user.Role.ToString());
		}

		public bool IsAuthorized(Guid id, string Token)
		{
			AccountDto account = _userManagementService.FindValidUserID(id);
			LoginTokenDto token = _baseService.FindToken(id, Token);
			if (account != null && token !=null && token.Token == Token)
				return true;
			return false;
		}
	}
}

