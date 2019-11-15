using Newtonsoft.Json;
using PasswordServerApi.Databases.DataModels;
using PasswordServerApi.DataFileDb;
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
	public class StorageServiceFile : IStorageService
	{
		public IReadFileDb _fileReader;

		public StorageServiceFile(IReadFileDb fileReader)
		{
			_fileReader = fileReader;
		}

		#region Account

		public List<AccountDto> GetAccounts()
		{
			List<AccountDto> accounts = new List<AccountDto>();
			_fileReader.ReadAccountFile()?.ToList().ForEach(x => accounts.Add(GetAccountDto(JsonConvert.DeserializeObject<AccountModel>(x.JsonData))));
			return accounts;
		}

		public AccountDto SetAccount(AccountDto addAccount)
		{
			List<AccountDto> accounts = GetAccounts();
			if (accounts.Find(x => x.AccountId == addAccount.AccountId) != null)
				return UpdateAccount(addAccount);
			else
				return AddNewAccount(addAccount);
		}

		#region Set AccountDto Helpers

		private AccountDto UpdateAccount(AccountDto accountDto)
		{
			AccountModel updateAccount = GetAccountModel(accountDto);
			EndityAbstractModelAccount AccountModelData = _fileReader.ReadAccountFile()?.ToList().Find(x => x.EndityId == updateAccount.AccountId);
			AccountModel dbAccountModel = JsonConvert.DeserializeObject<AccountModel>(AccountModelData.JsonData);

			AccountModelData.JsonData = JsonConvert.SerializeObject(updateAccount);
			return SaveFileAccount(AccountModelData);
		}

		private AccountDto AddNewAccount(AccountDto request)
		{
			return SaveFileAccount(new EndityAbstractModelAccount() { EndityId = request.AccountId.ToString(), JsonData = JsonConvert.SerializeObject(GetAccountModel(request)) });
		}

		private AccountDto SaveFileAccount(EndityAbstractModelAccount data)
		{
			List<EndityAbstractModelAccount> accountsModels = _fileReader.ReadAccountFile()?.ToList();
			EndityAbstractModelAccount accountModel = accountsModels?.Find(x => x.EndityId == data.EndityId);
			if (accountModel != null)
			{
				int index = accountsModels.FindIndex(x => x.EndityId == data.EndityId);
				accountsModels.RemoveAt(index);
			}
			accountsModels.Add(data);
			return GetAccountDto(JsonConvert.DeserializeObject<AccountModel>(_fileReader.WriteAccountFile(accountsModels)?.ToList().Find(x => x.EndityId == data.EndityId).JsonData));
		}


		#endregion

		public void DeleteAccount(AccountDto addAccount)
		{
			List<EndityAbstractModelAccount> accountsModels = _fileReader.ReadAccountFile()?.ToList();
			var accountToRemove = accountsModels.Find(x => x.EndityId == addAccount.AccountId.ToString());
			EndityAbstractModelAccount accountModel = accountsModels?.Find(x => x.EndityId == accountToRemove.EndityId);
			if (accountModel != null)
			{
				int index = accountsModels.FindIndex(x => x.EndityId == accountToRemove.EndityId);
				accountsModels.RemoveAt(index);
			}
			_fileReader.WriteAccountFile(accountsModels);
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
				Password = dtoAccount.Password,
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
			List<PasswordDto> Passwords = new List<PasswordDto>();
			_fileReader.ReadPasswordFile()?.ToList().ForEach(x => Passwords.Add(GetPasswordDto(JsonConvert.DeserializeObject<PasswordModel>(x.JsonData))));
			return Passwords;
		}

		public PasswordDto SetPassword(PasswordDto addPassword)
		{
			List<PasswordDto> passwords = GetPasswords();
			if (passwords.Find(x => x.PasswordId == addPassword.PasswordId) != null)
				return UpdatePassword(addPassword);
			else
				return AddNewPassword(addPassword);
		}

		private PasswordDto UpdatePassword(PasswordDto PasswordDto)
		{
			PasswordModel updatePassword = GetPasswordModel(PasswordDto);
			EndityAbstractModelPassword PasswordModelData = _fileReader.ReadPasswordFile()?.ToList().Find(x => x.EndityId == updatePassword.PasswordId);
			PasswordModel dbAccountModel = JsonConvert.DeserializeObject<PasswordModel>(PasswordModelData.JsonData);

			PasswordModelData.JsonData = JsonConvert.SerializeObject(updatePassword);
			return SaveFilePass(PasswordModelData);
		}

		private PasswordDto AddNewPassword(PasswordDto request)
		{
			return SaveFilePass(new EndityAbstractModelPassword() { EndityId = request.PasswordId.ToString(), JsonData = JsonConvert.SerializeObject(GetPasswordModel(request)) });
		}

		private PasswordDto SaveFilePass(EndityAbstractModelPassword data)
		{
			List<EndityAbstractModelPassword> passwordsModels = _fileReader.ReadPasswordFile()?.ToList();
			EndityAbstractModelPassword passwordModel = passwordsModels?.Find(x => x.EndityId == data.EndityId);
			if (passwordModel != null)
			{
				int index = passwordsModels.FindIndex(x => x.EndityId == data.EndityId);
				passwordsModels.RemoveAt(index);
			}
			passwordsModels.Add(data);
			return GetPasswordDto(JsonConvert.DeserializeObject<PasswordModel>(_fileReader.WritePasswordFile(passwordsModels)?.ToList().Find(x => x.EndityId == data.EndityId).JsonData));
		}


		public void DeletePassword(PasswordDto addPassword)
		{
			List<EndityAbstractModelPassword> passwordsModels = _fileReader.ReadPasswordFile()?.ToList();
			var passwordToRemove = passwordsModels.Find(x => x.EndityId == addPassword.PasswordId.ToString());
			EndityAbstractModelPassword passwordModel = passwordsModels?.Find(x => x.EndityId == passwordToRemove.EndityId);
			if (passwordModel != null)
			{
				int index = passwordsModels.FindIndex(x => x.EndityId == passwordToRemove.EndityId);
				passwordsModels.RemoveAt(index);
			}
			_fileReader.WritePasswordFile(passwordsModels);
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
			List<LoginTokenDto> Tokens = new List<LoginTokenDto>();
			_fileReader.ReadFileTokenToken()?.ToList().ForEach(x => Tokens.Add(GetLoginTokenDtoFromModel(x)));
			return Tokens;
		}

		public List<LoginTokenDto> SetToken(LoginTokenDto loginToken)
		{
			List<LoginTokenDto> Tokens = new List<LoginTokenDto>();
			List<LoginTokenModel> TokenModels = _fileReader.ReadFileTokenToken().ToList();
			int index = TokenModels.FindIndex(x => x.LoginTokenId == loginToken.LoginTokenId.ToString());
			if (index > 0 && index < TokenModels.Count)
			{
				TokenModels.RemoveAt(index);
			}
			loginToken.LastLogIn = DateTime.Now;
			TokenModels.Add(GetLoginTokenModelFromDto(loginToken));
			_fileReader.WriteFileToken(TokenModels)?.ToList().ForEach(x => Tokens.Add(GetLoginTokenDtoFromModel(x)));
			return Tokens;
		}

		public void DeleteToken(LoginTokenDto loginToken)
		{
			List<LoginTokenDto> Tokens = new List<LoginTokenDto>();
			List<LoginTokenModel> TokenModels = _fileReader.ReadFileTokenToken().ToList();
			int index = TokenModels.FindIndex(x => x.LoginTokenId == loginToken.LoginTokenId.ToString());
			if (index > 0 && index < TokenModels.Count)
			{
				TokenModels.RemoveAt(index);
			}
			_fileReader.WriteFileToken(TokenModels);
		}

		#region Helpers

		private LoginTokenDto GetLoginTokenDtoFromModel(LoginTokenModel model)
		{
			return new LoginTokenDto
			{
				LoginTokenId = Guid.Parse(model.LoginTokenId),
				UserId = Guid.Parse(model.UserId),
				Token = model.Token,
				UserAgend = model.UserAgend,
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
				UserAgend = model.UserAgend,
				LastLogIn = model.LastLogIn
			};
		}

		#endregion

		#endregion

	}
}
