using PasswordServerApi.DTO;
using PasswordServerApi.Interfaces;
using PasswordServerApi.Models.Enums;
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
				Password = $"123{i}",
				Sex = Sex.Male,
				LastLogIn = null,
				Passwords = new List<PasswordDto>() { },
			};
		}


	}
}
