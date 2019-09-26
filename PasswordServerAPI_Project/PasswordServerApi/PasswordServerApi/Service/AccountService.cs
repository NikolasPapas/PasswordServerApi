using System.Collections.Generic;
using PasswordServerApi.DataSqliteDB;
using PasswordServerApi.DTO;
using PasswordServerApi.Interfaces;
using PasswordServerApi.DataSqliteDB.DataModels;
using System;
using System.Linq;
using Newtonsoft.Json;

namespace PasswordServerApi.Service
{
	public class AccountService : IAccountService
	{
		IBaseService _baseService;
		ApplicationDbContext _dbContext;

		public AccountService(IBaseService baseService, ApplicationDbContext dbContext)
		{
			_baseService = baseService;
			_dbContext = dbContext;
		}

		public IEnumerable<AccountDto> GetAccounts()
		{
			List<AccountDto> accountds = new List<AccountDto>();
			_dbContext.Accounts.ToList().ForEach(x => accountds.Add(GetAccountDto(JsonConvert.DeserializeObject<AccountModel>(x))));
			return accountds;
		}

		public AccountDto UpdateAccount(AccountDto accountDto)
		{
			_dbContext.Update(GetAccount(accountDto));
			return accountDto;
		}

		public AccountDto GetAccount(Guid id)
		{
			return GetAccountDto(JsonConvert.DeserializeObject<AccountModel>((_dbContext.Accounts.ToList().Find(x => Guid.Parse(JsonConvert.DeserializeObject<AccountModel>(x).AccountId) == id))));
		}

		private AccountDto GetAccountDto(AccountModel dbAccount)
		{
			return new AccountDto()
			{
				AccountId = Guid.Parse(dbAccount.AccountId),
				FirstName = dbAccount.FirstName,
				LastName = dbAccount.LastName,
				UserName = dbAccount.UserName,
				Email = dbAccount.Email,
				Role = dbAccount.Role,
				Password = dbAccount.Password,
				Sex = dbAccount.Sex,
				LastLogIn = dbAccount.LastLogIn,
				Passwords = new List<PasswordDto>() { },
			};
		}

		private AccountModel GetAccount(AccountDto dtoAccount)
		{
			return new AccountModel()
			{
				AccountId = dtoAccount.AccountId.ToString(),
				FirstName = dtoAccount.FirstName,
				LastName = dtoAccount.LastName,
				UserName = dtoAccount.UserName,
				Email = dtoAccount.Email,
				Role = dtoAccount.Role,
				Password = dtoAccount.Password,
				Sex = dtoAccount.Sex,
				LastLogIn = dtoAccount.LastLogIn,
				PasswordIds = dtoAccount.Passwords.Select(x => x.PasswordId.ToString()).ToList(),
			};
		}
	}
}
