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
		IBaseService _baseService;

		public AccountService( IBaseService baseService)
		{
			_baseService = baseService;
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
			AccountDto savedAccount = _baseService.GetAccounts(request).FirstOrDefault();
			if (!StaticConfiguration.GetAcrionByRole.ContainsKey(savedAccount.Role))
				throw new Exception("Invalid Profile");
			else
			{
				ApplicationAction actions = StaticConfiguration.GetAcrionByRole[savedAccount.Role].Find(x => x.Id == request.ActionId);
				if (actions == null)
					throw new Exception("Invalid Action");
				Func<AccountDto, AccountDto, Response<AccountDto>> func;
				if (!this.ActionIdToFunction.TryGetValue(request.ActionId, out func)) throw new Exception("Δεν βρέθηκε ενέργεια για το Id: " + request.ActionId);

				Response < AccountDto > results = func(savedAccount, request.Account);
				results.SelectedAction = request.ActionId;
				return results;
			}
		}

		#region Actions

		private Response<AccountDto> SeveAccountFunc(AccountDto savedAccount, AccountDto requestedAccount)
		{
			if (savedAccount.AccountId == null)
				throw new Exception("NoAccountID ForUpdate");
			requestedAccount.AccountId = savedAccount.AccountId;
			return new Response<AccountDto>()
			{
				Payload = _baseService.UpdateAccount(requestedAccount,false),
				Warnnings = new List<string>()
			};
		}


		private Response<AccountDto> GetAccountAndPaswordsFunc(AccountDto savedAccount, AccountDto requestedAccount)
		{
			if (savedAccount.AccountId == null)
				throw new Exception("NoAccountID ForUpdate");
			List<PasswordDto> passwords = new List<PasswordDto>();
			savedAccount.Passwords.ForEach(x => passwords.Add(_baseService.GetPassword(x.PasswordId)));
			savedAccount.Passwords = passwords;
			return new Response<AccountDto>()
			{
				Payload = savedAccount
			};
		}

		#endregion

	}
}
