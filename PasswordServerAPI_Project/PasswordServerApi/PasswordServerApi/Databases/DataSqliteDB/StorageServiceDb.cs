using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using PasswordServerApi.Databases.DataModels;
using PasswordServerApi.DataSqliteDB;
using PasswordServerApi.DataSqliteDB.DataModels;
using PasswordServerApi.DTO;
using PasswordServerApi.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.StorageLayer
{
	public class StorageServiceDb : IStorageService
	{
		ApplicationDbContext _dbContext;
		private readonly IPasswordHasher<AccountDto> _passwordHasher;

		public bool ValidateAccount(AccountDto validateAccount)
		{
			List<AccountDto> accounts = new List<AccountDto>();
			_dbContext?.Accounts?.ToList()?.ForEach(x => accounts.Add(GetAccountDto(JsonConvert.DeserializeObject<AccountModel>(x.JsonData), true)));
			AccountDto validAccount = accounts.Find(x => x.UserName == validateAccount.UserName);
			return validAccount != null && _passwordHasher.VerifyHashedPassword(validAccount, validAccount.Password, validateAccount?.Password) == PasswordVerificationResult.Success;
		}

		public StorageServiceDb(ApplicationDbContext dbContext, IPasswordHasher<AccountDto> passwordHasher)
		{
			_dbContext = dbContext;
			_passwordHasher = passwordHasher;
		}

		#region Account

		public List<AccountDto> GetAccounts()
		{
			List<AccountDto> accounts = new List<AccountDto>();
			_dbContext.Accounts.ToList().ForEach(x => accounts.Add(GetAccountDto(JsonConvert.DeserializeObject<AccountModel>(x.JsonData))));
			return accounts;
		}

		public AccountDto SetAccount(AccountDto addAccount, string password)
		{
			List<AccountDto> accounts = GetAccounts();
			if (accounts.Find(x => x.AccountId == addAccount.AccountId) != null)
				return UpdateAccount(addAccount, password);
			else
				return AddNewAccount(addAccount);
		}

		#region Set AccountDto Helpers

		public AccountDto UpdateAccount(AccountDto accountDto, string password)
		{
			AccountModel updateAccount = GetAccountModel(accountDto);
			EndityAbstractModelAccount AccountModelData = _dbContext.Accounts.ToList().Find(x => x.EndityId == updateAccount.AccountId);
			AccountModel dbAccountModel = JsonConvert.DeserializeObject<AccountModel>(AccountModelData.JsonData);


			updateAccount.Password = dbAccountModel.Password;
			updateAccount.UserName = dbAccountModel.UserName;
			updateAccount.Email = dbAccountModel.Email;
			if (!string.IsNullOrWhiteSpace(password))
				updateAccount.Password = _passwordHasher.HashPassword(accountDto, password);

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


		public void DeleteAccount(AccountDto addAccount)
		{
			var accountToRemove = _dbContext.Accounts.ToList().Find(x => x.EndityId == addAccount.AccountId.ToString());
			_dbContext.Accounts.Remove(accountToRemove);
			_dbContext.SaveChanges();
		}



		#region Account Converter

		private AccountDto GetAccountDto(AccountModel dbAccount ,bool  full =false)
		{
			string password = "";
			if (full)
				password = dbAccount.Password;
			return new AccountDto()
			{
				AccountId = Guid.Parse(dbAccount.AccountId),
				FirstName = dbAccount.FirstName,
				LastName = dbAccount.LastName,
				UserName = dbAccount.UserName,
				Email = dbAccount.Email,
				Role = dbAccount.Role,
				Password = password,
				Sex = dbAccount.Sex,
				LastLogIn = dbAccount.LastLogIn,
				//CurrentToken = dbAccount.CurrentToken,
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
				Password = _passwordHasher.HashPassword(dtoAccount, dtoAccount.Password),
				Sex = dtoAccount.Sex,
				LastLogIn = dtoAccount.LastLogIn,
				//CurrentToken = dtoAccount.CurrentToken,
				PasswordIds = dtoAccount.Passwords.Select(x => x.PasswordId.ToString()).ToList(),
			};
		}

		#endregion

		#endregion

		#region Password

		public List<PasswordDto> GetPasswords()
		{
			List<PasswordDto> passwords = new List<PasswordDto>();
			_dbContext.Passwords.ToList().ForEach(x => passwords.Add(GetPasswordDto(JsonConvert.DeserializeObject<PasswordModel>(x.JsonData))));
			return passwords;
		}

		public PasswordDto SetPassword(PasswordDto addPassword)
		{
			List<PasswordDto> passwords = GetPasswords();
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

		public void DeletePassword(PasswordDto addPassword)
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

		#region Tokens

		public List<LoginTokenDto> GetTokens()
		{
			List<LoginTokenDto> tokens = new List<LoginTokenDto>();
			_dbContext.LoginTokens.ToList().ForEach(x => tokens.Add(GetLoginTokenDtoFromModel(x)));
			return tokens;
		}

		public List<LoginTokenDto> SetToken(LoginTokenDto loginToken)
		{
			List<LoginTokenDto> Tokens = GetTokens();
			if (Tokens.Find(x => x.LoginTokenId == loginToken.LoginTokenId) != null)
				DeleteToken(loginToken);
			return AddNewToken(loginToken);
		}

		public List<LoginTokenDto> AddNewToken(LoginTokenDto loginToken)
		{
			var newToken = GetLoginTokenModelFromDto(loginToken);
			_dbContext.LoginTokens.Add(newToken);
			_dbContext.SaveChanges();
			return GetTokens();
		}

		public void DeleteToken(LoginTokenDto loginToken)
		{
			var tokenToRemove = _dbContext.LoginTokens.ToList().Find(x => x.LoginTokenId == loginToken.LoginTokenId.ToString());
			_dbContext.LoginTokens.Remove(tokenToRemove);
			_dbContext.SaveChanges();
		}

		#region Helpers

		private LoginTokenDto GetLoginTokenDtoFromModel(LoginTokenModel model)
		{
			return new LoginTokenDto
			{
				LoginTokenId = Guid.Parse(model.LoginTokenId),
				UserId = Guid.Parse(model.UserId),
				Token = model.Token,
				UserAgent = model.UserAgent,
				LastLogIn = model.LastLogIn
			};
		}


		private LoginTokenModel GetLoginTokenModelFromDto(LoginTokenDto model)
		{
			return new LoginTokenModel
			{
				LoginTokenId = model.LoginTokenId.ToString(),
				UserId = model.UserId.ToString(),
				Token = model.Token,
				UserAgent = model.UserAgent,
				LastLogIn = model.LastLogIn
			};
		}

		#endregion

		#endregion

	}
}
