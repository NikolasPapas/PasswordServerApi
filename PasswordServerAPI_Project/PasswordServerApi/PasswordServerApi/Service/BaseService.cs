using Newtonsoft.Json;
using PasswordServerApi.DataSqliteDB;
using PasswordServerApi.DataSqliteDB.DataModels;
using PasswordServerApi.DTO;
using PasswordServerApi.Interfaces;
using PasswordServerApi.Models.Account.Requests;
using PasswordServerApi.Models.Enums;
using PasswordServerApi.Models.Requests.Password;
using PasswordServerApi.Security.SecurityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.Service
{
	public class BaseService : IBaseService
	{
		ApplicationDbContext _dbContext;
		public BaseService(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		#region Database Connections Account

		public AccountDto GetSpesificAccount(AccountActionRequest request)
		{
			List<AccountDto> accounts = new List<AccountDto>();
			_dbContext.Accounts.ToList().ForEach(x => accounts.Add(GetAccountDto(JsonConvert.DeserializeObject<AccountModel>(x.JsonData))));

			return accounts.Find(x => request?.Account?.UserName == x.UserName && request?.Account?.Password == x.Password);
		}

		public IEnumerable<AccountDto> GetAccounts(AccountActionRequest request)
		{
			List<AccountDto> accounts = new List<AccountDto>();
			_dbContext.Accounts.ToList().ForEach(x => accounts.Add(GetAccountDto(JsonConvert.DeserializeObject<AccountModel>(x.JsonData))));
			List<AccountDto> filteredAccounts = new List<AccountDto>();

			var filtered = accounts.FindAll(x =>
			{
				bool isCorrectAccount = false;
				isCorrectAccount = (!string.IsNullOrWhiteSpace(request?.Account?.UserName) ? x.UserName == request?.Account?.UserName : true) &&
									(!string.IsNullOrWhiteSpace(request?.Account?.Email) ? x.Email == request?.Account?.Email : true) &&
									(!string.IsNullOrWhiteSpace(request?.Account?.FirstName) ? x.FirstName == request?.Account?.FirstName : true) &&
									(!string.IsNullOrWhiteSpace(request?.Account?.LastName) ? x.FirstName == request?.Account?.LastName : true) &&
									(!string.IsNullOrWhiteSpace(request?.Account?.Role) ? x.Role == request?.Account?.Role : true);
				return isCorrectAccount;
			});
			return filtered.Select(x => { if (request?.Account?.Password == null) { x.Password = ""; x.CurentToken = ""; } return x; });
		}

		public AccountDto UpdateAccount(AccountDto accountDto, bool full = false)
		{
			AccountModel updateAccount = GetAccountModel(accountDto);
			EndityAbstractModelAccount AccountModelData = _dbContext.Accounts.ToList().Find(x => x.EndityId == updateAccount.AccountId);
			AccountModel dbAccountModel = JsonConvert.DeserializeObject<AccountModel>(AccountModelData.JsonData);
			if (updateAccount.Password != dbAccountModel.Password)
				throw new Exception("Invalid Password");

			dbAccountModel.Email = updateAccount.Email;
			dbAccountModel.FirstName = updateAccount.FirstName;
			dbAccountModel.LastName = updateAccount.LastName;
			dbAccountModel.Sex = updateAccount.Sex;

			if (full)
			{
				dbAccountModel.LastLogIn = updateAccount.LastLogIn;
				dbAccountModel.Role = updateAccount.Role;
				dbAccountModel.CurentToken = updateAccount.CurentToken;
				dbAccountModel.Password = updateAccount.Password;
				dbAccountModel.AccountId = updateAccount.AccountId;
				dbAccountModel.PasswordIds = updateAccount.PasswordIds;
			}

			AccountModelData.JsonData = JsonConvert.SerializeObject(dbAccountModel);
			_dbContext.Accounts.Update(AccountModelData);
			_dbContext.SaveChanges();

			return accountDto;
		}

		public AccountDto GetAccountById(Guid id)
		{
			return GetAccountDto(JsonConvert.DeserializeObject<AccountModel>((_dbContext.Accounts.ToList().Find(x => Guid.Parse(x.EndityId) == id)).JsonData));
		}

		public AccountDto AddNewAccount(AccountDto request)
		{
			_dbContext.Accounts.Add(new EndityAbstractModelAccount() { EndityId = request.AccountId.ToString(), JsonData = JsonConvert.SerializeObject(GetAccountModel(request)) });
			_dbContext.SaveChanges();
			return request;
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
				CurentToken = dbAccount.CurentToken,
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
				CurentToken = dtoAccount.CurentToken,
				PasswordIds = dtoAccount.Passwords.Select(x => x.PasswordId.ToString()).ToList(),
			};
		}

		#endregion

		#endregion

		#region Database Connections Passwords

		public IEnumerable<PasswordDto> GetPasswords(PasswordActionRequest request, AccountDto account)
		{
			List<PasswordDto> passwords = new List<PasswordDto>();
			_dbContext.Passwords.ToList().ForEach(x =>
			{
				if (account.Passwords.Find(accountPass => accountPass.PasswordId.ToString() == x.EndityId) == null)
					return;
				var passwordModel = JsonConvert.DeserializeObject<PasswordModel>(x.JsonData);
				bool haseCorrectValues = false;
				haseCorrectValues = (!string.IsNullOrWhiteSpace(request?.Password?.Name) ? request?.Password?.Name == passwordModel?.Name : true) && (!string.IsNullOrWhiteSpace(request?.Password?.LogInLink) ? request?.Password?.LogInLink == passwordModel?.LogInLink : true) && (!string.IsNullOrWhiteSpace(request?.Password?.UserName) ? request?.Password?.LogInLink == passwordModel?.LogInLink : true);
				if (haseCorrectValues)
					passwords.Add(GetPasswordDto(passwordModel));
			});
			return passwords;
		}

		public PasswordDto GetPassword(Guid id)
		{
			return GetPasswordDto(JsonConvert.DeserializeObject<PasswordModel>((_dbContext.Passwords.ToList().Find(x => Guid.Parse(x.EndityId) == id)).JsonData));
		}

		public PasswordDto UpdatePassword(PasswordDto passwordDto)
		{
			var updatePassword = GetPasswordModel(passwordDto);
			var passwordModelData = _dbContext.Passwords.ToList().Find(x => x.EndityId == updatePassword.PasswordId);
			passwordModelData.JsonData = JsonConvert.SerializeObject(updatePassword);
			_dbContext.Passwords.Update(passwordModelData);
			_dbContext.SaveChanges();
			return passwordDto;
		}

		public PasswordDto AddNewPassword(PasswordDto requestPassword)
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
				Strength = dbPassword.Strength
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
