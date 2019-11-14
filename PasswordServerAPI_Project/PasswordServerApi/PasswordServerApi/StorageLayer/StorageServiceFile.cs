using Newtonsoft.Json;
using PasswordServerApi.DataFileDb;
using PasswordServerApi.DataSqliteDB;
using PasswordServerApi.DataSqliteDB.DataModels;
using PasswordServerApi.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.StorageLayer
{
	public class StorageServiceFile : IStorageService
	{
		private IApplicationFileDb _applicationFileDb;

		public StorageServiceFile(IApplicationFileDb applicationFileDb)
		{
			_applicationFileDb = applicationFileDb;
		}

		#region Account

		public List<AccountDto> GetAccountsDto()
		{
			List<AccountDto> accounts = new List<AccountDto>();
			_applicationFileDb.ReadDbAccountFile()?.ToList().ForEach(x => accounts.Add(GetAccountDto(JsonConvert.DeserializeObject<AccountModel>(x.JsonData))));
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

		private AccountDto UpdateAccount(AccountDto accountDto)
		{
			AccountModel updateAccount = GetAccountModel(accountDto);
			EndityAbstractModelAccount AccountModelData = _applicationFileDb.ReadDbAccountFile()?.ToList().Find(x => x.EndityId == updateAccount.AccountId);
			AccountModel dbAccountModel = JsonConvert.DeserializeObject<AccountModel>(AccountModelData.JsonData);

			AccountModelData.JsonData = JsonConvert.SerializeObject(updateAccount);
			_applicationFileDb.SaveToAccountFile(AccountModelData);

			return accountDto;
		}

		private AccountDto AddNewAccount(AccountDto request)
		{
			_applicationFileDb.SaveToAccountFile(new EndityAbstractModelAccount() { EndityId = request.AccountId.ToString(), JsonData = JsonConvert.SerializeObject(GetAccountModel(request)) });
			return request;
		}

		#endregion

		public void DeleteAccountsDto(AccountDto addAccount)
		{
			var accountToRemove = _applicationFileDb.ReadDbAccountFile()?.ToList().Find(x => x.EndityId == addAccount.AccountId.ToString());
			_applicationFileDb.DeleteToAccountFile(accountToRemove);
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

		public List<PasswordDto>  GetPasswordsDto()
		{
			List<PasswordDto> Passwords = new List<PasswordDto>();
			_applicationFileDb.ReadDbPasswordFile()?.ToList().ForEach(x => Passwords.Add(GetPasswordDto(JsonConvert.DeserializeObject<PasswordModel>(x.JsonData))));
			return Passwords;
		}

		public PasswordDto SetPasswordsDto(PasswordDto addPassword)
		{
			List<PasswordDto> passwords = GetPasswordsDto();
			if (passwords.Find(x => x.PasswordId == addPassword.PasswordId) != null)
				return UpdatePassword(addPassword);
			else
				return AddNewPassword(addPassword);
		}

		private PasswordDto UpdatePassword(PasswordDto PasswordDto)
		{
			PasswordModel updatePassword = GetPasswordModel(PasswordDto);
			EndityAbstractModelPassword PasswordModelData = _applicationFileDb.ReadDbPasswordFile()?.ToList().Find(x => x.EndityId == updatePassword.PasswordId);
			PasswordModel dbAccountModel = JsonConvert.DeserializeObject<PasswordModel>(PasswordModelData.JsonData);

			PasswordModelData.JsonData = JsonConvert.SerializeObject(updatePassword);
			_applicationFileDb.SaveToPasswordFile(PasswordModelData);
			return PasswordDto;
		}

		private PasswordDto AddNewPassword(PasswordDto request)
		{
			_applicationFileDb.SaveToPasswordFile(new EndityAbstractModelPassword() { EndityId = request.PasswordId.ToString(), JsonData = JsonConvert.SerializeObject(GetPasswordModel(request)) });
			return request;
		}

		public void DeletePasswordsDto(PasswordDto addPassword)
		{
			var passwordToRemove = _applicationFileDb.ReadDbPasswordFile()?.ToList().Find(x => x.EndityId == addPassword.PasswordId.ToString());
			_applicationFileDb.DeleteToPasswordFile(passwordToRemove);
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
