﻿using PasswordServerApi.Security.SecurityModels;
using System;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using PasswordServerApi.DTO;
using PasswordServerApi.Models;
using System.Collections.Generic;
using PasswordServerApi.Service;

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

		public List<ApplicationAction> IsAuthenticated(TokenRequest request, out string token)
		{
			List<ApplicationAction> ActionList =new List<ApplicationAction>();
			token = string.Empty;
			AccountDto user = _userManagementService.FindValidUser(request.Username, request.Password);
			if (user == null) return null;
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
				expires: DateTime.Now.AddMinutes(3),
				signingCredentials: credentials
			);
			token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

			_userManagementService.SaveNewToken(user.AccountId, token);
			if (StaticConfiguration.GetAcrionByRole.ContainsKey(user.Role.ToString()))
				ActionList = StaticConfiguration.GetAcrionByRole[user.Role.ToString()];
			return ActionList;

		}


		public bool IsAuthorized(Guid id, string Token)
		{
			AccountDto account = _userManagementService.FindValidUserID(id);
			if (account != null && account.CurentToken == Token)
				return true;
			return false;
		}
	}
}

