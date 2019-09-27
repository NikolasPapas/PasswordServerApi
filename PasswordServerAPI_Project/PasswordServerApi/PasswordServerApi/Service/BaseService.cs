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

		public IEnumerable<AccountDto> GetAccounts(SearchAccountsRequest request)
		{
			List<AccountDto> accounts = new List<AccountDto>();
			_dbContext.Accounts.ToList().ForEach(x => accounts.Add(GetAccountDto(JsonConvert.DeserializeObject<AccountModel>(x.JsonData))));
			List<AccountDto> filteredAccounts = new List<AccountDto>();

			var filtered = accounts.FindAll(x =>
			{
				bool isCorrectAccount = false;
				isCorrectAccount = (x.UserName == request?.UserName) || (x.Email == request?.Email);

				if (request.Password != null)
					isCorrectAccount = x.Password == request?.Password && x.UserName == request?.UserName;
				return isCorrectAccount;
			});
			return filtered.Select(x => { if (request.Password == null) { x.Password = ""; x.CurentToken = ""; } return x; });
		}

		private AccountDto GetAccountDto(object p)
		{
			throw new NotImplementedException();
		}

		public AccountDto UpdateAccount(AccountDto accountDto, bool full =false)
		{
			AccountModel updateAccount = GetAccountModel(accountDto);
			EndityAbstractModelAccount AccountModelData = _dbContext.Accounts.ToList().Find(x => x.EndityId == updateAccount.AccountId);
			AccountModel dbAccountModel = JsonConvert.DeserializeObject<AccountModel>(AccountModelData.JsonData);
			if (updateAccount.Password != dbAccountModel.Password)
				throw new Exception("Invalid Password");

			dbAccountModel.Email = updateAccount.Email;
			dbAccountModel.FirstName = updateAccount.FirstName;
			dbAccountModel.LastName = updateAccount.LastName;
			dbAccountModel.Role = updateAccount.Role;
			dbAccountModel.Sex = updateAccount.Sex;

			if (full)
			{
				dbAccountModel.CurentToken = updateAccount.CurentToken;
				dbAccountModel.Password = updateAccount.Password;
				dbAccountModel.AccountId = updateAccount.AccountId;
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

		public IEnumerable<PasswordDto> GetPasswords(PasswordActionRequest request)
		{
			List<PasswordDto> passwords = new List<PasswordDto>();
			_dbContext.Passwords.ToList().ForEach(x =>
			{
				var passwordModel = JsonConvert.DeserializeObject<PasswordModel>(x.JsonData);
				bool haseCorrectValues = false;
				haseCorrectValues = request.Name == passwordModel.Name || request.LogInLink == passwordModel.LogInLink;
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
			var passwordModelData = _dbContext.Passwords.ToList().Find(x => x.EndityId == updatePassword.Password);
			passwordModelData.JsonData = JsonConvert.SerializeObject(updatePassword);
			_dbContext.Passwords.Update(passwordModelData);
			_dbContext.SaveChanges();
			return passwordDto;
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
