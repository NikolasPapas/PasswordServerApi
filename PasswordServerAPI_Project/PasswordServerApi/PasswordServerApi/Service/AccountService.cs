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

		public AccountService(IBaseService baseService)
		{
			_baseService = baseService;
		}

		#region Dictionary ActionId To Function

		private readonly Dictionary<Guid, Func<AccountDto, AccountDto, AccountActionRequest, Response<List<AccountDto>>>> _actionIdToFunction;

		private Dictionary<Guid, Func<AccountDto, AccountDto, AccountActionRequest, Response<List<AccountDto>>>> ActionIdToFunction
		{
			get
			{
				return _actionIdToFunction ??
					new Dictionary<Guid, Func<AccountDto, AccountDto, AccountActionRequest, Response<List<AccountDto>>>>()
					{
						{ StaticConfiguration.ActionSaveAccountId, ActionSeveAccountFunc },
						{ StaticConfiguration.ActionGetAccountId, ActionGetAccountFunc },
						{ StaticConfiguration.ActionAddNewAccountId, ActionAddNewAccountFunc },
						{ StaticConfiguration.ActionGetAccountAndPasswordId, ActionGetAccountAndPasswordFunc },
						{ StaticConfiguration.ActionRemoveAccountId, ActionRemoveAccountFunc },

					};
			}
		}

		#endregion

		public Response<List<AccountDto>> ExecuteAction(AccountActionRequest request)
		{
			if (request.AccountId == null)
				throw new Exception("No AccountID");
			AccountDto savedAccount = _baseService.GetAccountById(request.AccountId);
			if (!StaticConfiguration.GetAcrionByRole.ContainsKey(savedAccount.Role))
				throw new Exception("Invalid Profile");
			else
			{
				ApplicationAction actions = StaticConfiguration.GetAcrionByRole[savedAccount.Role].Find(x => x.Id == request.ActionId);
				if (actions == null)
					throw new Exception("Invalid Action");
				Func<AccountDto, AccountDto, AccountActionRequest, Response<List<AccountDto>>> func;
				if (!this.ActionIdToFunction.TryGetValue(request.ActionId, out func)) throw new Exception("Δεν βρέθηκε ενέργεια για το Id: " + request.ActionId);

				Response<List<AccountDto>> results = func(savedAccount, request.Account, request);
				results.SelectedAction = request.ActionId;
				return results;
			}
		}

		#region Actions

		private Response<List<AccountDto>> ActionSeveAccountFunc(AccountDto savedAccount, AccountDto requestedAccount, AccountActionRequest request)
		{
			requestedAccount.AccountId = savedAccount.AccountId;
			return new Response<List<AccountDto>>() { Payload = new List<AccountDto>() { _baseService.UpdateAccount(requestedAccount, false) } };
		}

		private Response<List<AccountDto>> ActionGetAccountFunc(AccountDto savedAccount, AccountDto requestedAccount, AccountActionRequest request)
		{
			List<PasswordDto> passwords = new List<PasswordDto>();
			savedAccount.Passwords.ForEach(x => passwords.Add(_baseService.GetPassword(x.PasswordId)));
			savedAccount.Passwords = passwords;
			return new Response<List<AccountDto>>() { Payload = new List<AccountDto>() { savedAccount } };
		}

		private Response<List<AccountDto>> ActionAddNewAccountFunc(AccountDto savedAccount, AccountDto requestedAccount, AccountActionRequest request)
		{
			requestedAccount.AccountId = Guid.NewGuid();
			requestedAccount.Passwords = new List<PasswordDto>();

			if (_baseService.GetAccounts(new AccountActionRequest() { Account = new AccountDto() { UserName = requestedAccount?.UserName } }).ToList().Count > 0)
				throw new Exception("Rong Username");

			_baseService.AddNewAccount(requestedAccount);

			return new Response<List<AccountDto>>() { Payload = new List<AccountDto>() { requestedAccount } };
		}

		private Response<List<AccountDto>> ActionGetAccountAndPasswordFunc(AccountDto savedAccount, AccountDto requestedAccount, AccountActionRequest request)
		{

			return new Response<List<AccountDto>>() { Payload = _baseService.GetAccounts(request).ToList() };
		}

		private Response<List<AccountDto>> ActionRemoveAccountFunc(AccountDto savedAccount, AccountDto requestedAccount, AccountActionRequest request)
		{
			AccountDto accountToDelete = _baseService.GetAccountById(requestedAccount.AccountId);
			accountToDelete.Passwords.ForEach(password =>
			{
				password = _baseService.GetPassword(password.PasswordId);
				_baseService.RemovePassword(password);
			});
			_baseService.RemoveAccount(accountToDelete);

			return new Response<List<AccountDto>>() { Payload = new List<AccountDto> { accountToDelete } };
		}
		#endregion

	}
}
