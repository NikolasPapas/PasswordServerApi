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
		IPasswordService _passwordService;
		public AccountService(ApplicationDbContext dbContext, IPasswordService passwordService)
		{
			_dbContext = dbContext;
			_passwordService = passwordService;
		}

		#region Dictionary ActionId To Function

		private readonly Dictionary<Guid, Func<AccountDto, AccountDto, Response<AccountDto>>> _actionIdToFunction;

		private Dictionary<Guid, Func<AccountDto, AccountDto, Response<AccountDto>>> ActionIdToFunction
		{
			get
			{
				return _actionIdToFunction ??
					new Dictionary<Guid, Func<AccountDto, AccountDto, Response<AccountDto>>>()
					{
						{ StaticConfiguration.ActionSaveAccountId, SeveAccountFunc },
						{ StaticConfiguration.ActionGetAccountAndPasswordId, GetAccountAndPaswordsFunc },
					};
			}
		}

		#endregion

		public Response<AccountDto> ExecuteAction(AccountActionRequest request)
		{
			AccountDto savedAccount = GetAccounts(request).FirstOrDefault();
			if (!StaticConfiguration.GetAcrionByRole.ContainsKey(savedAccount.Role))
				throw new Exception("Invalid Profile");
			else
			{
				ApplicationAction actions = StaticConfiguration.GetAcrionByRole[savedAccount.Role].Find(x => x.Id == request.ActionId);
				if (actions == null)
					throw new Exception("Invalid Action");
				Func<AccountDto, AccountDto, Response<AccountDto>> func;
				if (!this.ActionIdToFunction.TryGetValue(request.ActionId, out func)) throw new Exception("Δεν βρέθηκε ενέργεια για το ActionId: " + request.ActionId);
				return func(savedAccount, request.Account);
			}
		}

		#region Actions

		private Response<AccountDto> SeveAccountFunc(AccountDto savedAccount, AccountDto requestedAccount)
		{
			if (requestedAccount.AccountId == null)
				throw new Exception("NoAccountID ForUpdate");

			return new Response<AccountDto>()
			{
				Payload = UpdateAccount(requestedAccount),
				Warnnings = new List<string>()
			};
		}


		private Response<AccountDto> GetAccountAndPaswordsFunc(AccountDto savedAccount, AccountDto requestedAccount)
		{
			if (savedAccount.AccountId == null)
				throw new Exception("NoAccountID ForUpdate");
			List<PasswordDto> passwords = new List<PasswordDto>();
			savedAccount.Passwords.ForEach(x => passwords.Add(_passwordService.GetPassword(x.PasswordId)));
			savedAccount.Passwords = passwords;
			return new Response<AccountDto>()
			{
				Payload = savedAccount
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
			EndityAbstractModelAccount AccountModelData = _dbContext.Accounts.ToList().Find(x => x.EndityId == updateAccount.AccountId);
			AccountModel dbAccountModel = JsonConvert.DeserializeObject<AccountModel>(AccountModelData.JsonData);
			if (updateAccount.Password != dbAccountModel.Password)
				throw new Exception("Invalid Password");

			dbAccountModel.Email = updateAccount.Email;
			dbAccountModel.FirstName = updateAccount.FirstName;
			dbAccountModel.LastName = updateAccount.LastName;
			dbAccountModel.Role = updateAccount.Role;
			dbAccountModel.Sex = updateAccount.Sex;

			AccountModelData.JsonData = JsonConvert.SerializeObject(dbAccountModel);
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
	}
}
