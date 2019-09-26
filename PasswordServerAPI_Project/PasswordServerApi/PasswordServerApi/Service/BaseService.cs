using PasswordServerApi.DTO;
using PasswordServerApi.Interfaces;
using PasswordServerApi.Models.Enums;
using PasswordServerApi.Security.SecurityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.Service
{
	public class BaseService : IBaseService
	{
		public BaseService()
		{

		}

		public PasswordDto GetDumyPassword(int i)
		{
			return new PasswordDto()
			{
				PasswordId = Guid.NewGuid(),
				Name = "Google" + i,
				UserName = $"nikolaspapazian{i}@gmail.com",
				Password = $"123{i}",
				LogInLink = "google.com",
				Sensitivity = Sensitivity.OnlyUser,
				Strength = Strength.VeryWeak
			};
		}

		public AccountDto GetDumyAccount(int i)
		{
			return new AccountDto()
			{
				AccountId = Guid.NewGuid(),
				FirstName = $"nikolas{i}",
				LastName = $"papazian{i}",
				UserName = $"npapazian{i}",
				Email = $"npapazian{i}@cite.gr",
				Role = i==1?Role.Admin:i==2?Role.User:Role.Viewer,
				Password = $"123{i}",
				Sex = Sex.Male,
				LastLogIn = null,
				Passwords = new List<PasswordDto>() { },
			};
		}


		public AccountDto GetDumyfullAccount(int i)
		{
			AccountDto results = GetDumyAccount(i);
			for (int dumyi = 0; dumyi <= i; dumyi++)
				results.Passwords.Add(GetDumyPassword(dumyi));
			return results;
		}

	}
}
