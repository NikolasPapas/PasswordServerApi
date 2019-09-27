using System.Collections.Generic;
using PasswordServerApi.DataSqliteDB;
using PasswordServerApi.DTO;
using PasswordServerApi.Interfaces;
using System;
using System.Linq;
using Newtonsoft.Json;
using PasswordServerApi.Models.Requests;
using PasswordServerApi.Models.Account.Requests;
using PasswordServerApi.Models;
using PasswordServerApi.Models.Responces;

namespace PasswordServerApi.Service
{
	public class AccountService : IAccountService
	{
		ApplicationDbContext _dbContext;

		public AccountService(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}


		#region Dictionary ActionId To Function
		private Dictionary<Guid, Func<AccountDto, string, Response<AccountDto>>> _actionIdToFunction;

		private Dictionary<Guid, Func<AccountDto, string, Response<AccountDto>>> ActionIdToFunction
		{
			get
			{
				return _actionIdToFunction ??
					new Dictionary<Guid, Func<AccountDto, string, Response<AccountDto>>>()
					{
						{ StaticConfiguration.ActionSaveAccountId, SeveAccountFunc }
					};
			}
		}

		#endregion






		public Response<AccountDto> ExecuteAction(AccountActionRequest request)
		{
			if (!StaticConfiguration.GetAcrionByRole.ContainsKey(request.Role))
				throw new Exception("Invalid Profile");
			else
			{
				ApplicationAction actions = StaticConfiguration.GetAcrionByRole[request.Role].Find(x => x.Id == request.ActionId);
				if (actions == null)
					throw new Exception("Invalid Action");
				Func<AccountDto, string, Response<AccountDto>> func;
				if (!this.ActionIdToFunction.TryGetValue(request.ActionId, out func)) throw new Exception("Δεν βρέθηκε ενέργεια για το ActionId: " + request.ActionId);
				return func(request.Account, request.Account.Password);
			}
		}


		#region Actions

		private Response<AccountDto> SeveAccountFunc(AccountDto account, string password)
		{
			if (account.AccountId == null)
				throw new Exception("NoAccountID ForUpdate");

			return new Response<AccountDto>()
			{
				Payload = UpdateAccount(account),
				Warnnings = new List<string>()
			};
		}

		#endregion


		#region Database Connections

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

		public AccountDto UpdateAccount(AccountDto accountDto)
		{
			AccountModel updateAccount = GetAccountModel(accountDto);
			var AccountModelData = _dbContext.Accounts.ToList().Find(x => x.EndityId == updateAccount.AccountId);
			if (updateAccount.Password != JsonConvert.DeserializeObject<AccountModel>(AccountModelData.JsonData).Password)
				throw new Exception("Invalid Password");
			AccountModelData.JsonData = JsonConvert.SerializeObject(updateAccount);
			_dbContext.Accounts.Update(AccountModelData);
			_dbContext.SaveChanges();

			return accountDto;
		}

		public AccountDto GetAccountById(Guid id)
		{
			return GetAccountDto(JsonConvert.DeserializeObject<AccountModel>((_dbContext.Accounts.ToList().Find(x => Guid.Parse(x.EndityId) == id)).JsonData));
		}

		#endregion


		#region Helper Transformer

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
				Passwords = new List<PasswordDto>() { },
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
	}
}
