using Newtonsoft.Json;
using PasswordServerApi.DataSqliteDB;
using PasswordServerApi.DataSqliteDB.DataModels;
using PasswordServerApi.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.StorageLayer
{
	public class StorageServiceDb : IStorageService
	{
		ApplicationDbContext _dbContext;

		public StorageServiceDb(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		#region Account

		public List<AccountDto> GetAccountsDto()
		{
			List<AccountDto> accounts = new List<AccountDto>();
			_dbContext.Accounts.ToList().ForEach(x => accounts.Add(GetAccountDto(JsonConvert.DeserializeObject<AccountModel>(x.JsonData))));
			return accounts;
		}

		public AccountDto SetAccountsDto(AccountDto addAccount)
		{
			List<AccountDto> accounts = GetAccountsDto();
			if (accounts.Find(x => x.AccountId == addAccount.AccountId) != null)
				return UpdateAccount(addAccount);
			else
				return AddNewAccount(addAccount);
		}

		#region Set AccountDto Helpers

		public AccountDto UpdateAccount(AccountDto accountDto)
		{
			AccountModel updateAccount = GetAccountModel(accountDto);
			EndityAbstractModelAccount AccountModelData = _dbContext.Accounts.ToList().Find(x => x.EndityId == updateAccount.AccountId);
			AccountModel dbAccountModel = JsonConvert.DeserializeObject<AccountModel>(AccountModelData.JsonData);

			AccountModelData.JsonData = JsonConvert.SerializeObject(updateAccount);
			_dbContext.Accounts.Update(AccountModelData);
			_dbContext.SaveChanges();

			return accountDto;
		}

		public AccountDto AddNewAccount(AccountDto request)
		{
			_dbContext.Accounts.Add(new EndityAbstractModelAccount() { EndityId = request.AccountId.ToString(), JsonData = JsonConvert.SerializeObject(GetAccountModel(request)) });
			_dbContext.SaveChanges();
			return request;
		}

		#endregion


		public void DeleteAccountsDto(AccountDto addAccount)
		{
			var accountToRemove = _dbContext.Accounts.ToList().Find(x => x.EndityId == addAccount.AccountId.ToString());
			_dbContext.Accounts.Remove(accountToRemove);
			_dbContext.SaveChanges();
		}



		#region Account Converter

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
				CurrentToken = dbAccount.CurrentToken,
				Passwords = dbAccount.PasswordIds.Select(x => { return new PasswordDto() { PasswordId = Guid.Parse(x) }; }).ToList(),
			};
		}

		private AccountModel GetAccountModel(AccountDto dtoAccount)
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
				CurrentToken = dtoAccount.CurrentToken,
				PasswordIds = dtoAccount.Passwords.Select(x => x.PasswordId.ToString()).ToList(),
			};
		}

		#endregion

		#endregion

		#region Password

		public List<PasswordDto> GetPasswordsDto()
		{
			List<PasswordDto> passwords = new List<PasswordDto>();
			_dbContext.Passwords.ToList().ForEach(x => passwords.Add(GetPasswordDto(JsonConvert.DeserializeObject<PasswordModel>(x.JsonData))));
			return passwords;
		}

		public PasswordDto SetPasswordsDto(PasswordDto addPassword)
		{
			List<PasswordDto> passwords = GetPasswordsDto();
			if (passwords.Find(x => x.PasswordId == addPassword.PasswordId) != null)
				return UpdatePassword(addPassword);
			else
				return AddNewPassword(addPassword);
		}

		#region Set Password Helpers

		private PasswordDto UpdatePassword(PasswordDto passwordDto)
		{
			var updatePassword = GetPasswordModel(passwordDto);
			var passwordModelData = _dbContext.Passwords.ToList().Find(x => x.EndityId == updatePassword.PasswordId);
			passwordModelData.JsonData = JsonConvert.SerializeObject(updatePassword);
			_dbContext.Passwords.Update(passwordModelData);
			_dbContext.SaveChanges();
			return passwordDto;
		}

		private PasswordDto AddNewPassword(PasswordDto requestPassword)
		{
			var newPassword = GetPasswordModel(requestPassword);
			_dbContext.Passwords.Add(new EndityAbstractModelPassword()
			{
				EndityId = requestPassword.PasswordId.ToString(),
				JsonData = JsonConvert.SerializeObject(newPassword)
			});
			_dbContext.SaveChanges();
			return requestPassword;
		}

		#endregion

		public void DeletePasswordsDto(PasswordDto addPassword)
		{
			var passToRemove = _dbContext.Passwords.ToList().Find(x => x.EndityId == addPassword.PasswordId.ToString());
			_dbContext.Passwords.Remove(passToRemove);
			_dbContext.SaveChanges();
		}

		#region Converters Password

		private PasswordDto GetPasswordDto(PasswordModel dbPassword)
		{
			return new PasswordDto()
			{
				PasswordId = Guid.Parse(dbPassword.PasswordId),
				Name = dbPassword.Name,
				UserName = dbPassword.UserName,
				Password = dbPassword.Password,
				LogInLink = dbPassword.LogInLink,
				Sensitivity = dbPassword.Sensitivity,
			};

		}

		private PasswordModel GetPasswordModel(PasswordDto dtoPassword)
		{
			return new PasswordModel()
			{
				PasswordId = dtoPassword.PasswordId.ToString(),
				Name = dtoPassword.Name,
				UserName = dtoPassword.UserName,
				Password = dtoPassword.Password,
				LogInLink = dtoPassword.LogInLink,
				Sensitivity = dtoPassword.Sensitivity,
				Strength = dtoPassword.Strength
			};
		}

		#endregion

		#endregion
	}
}
