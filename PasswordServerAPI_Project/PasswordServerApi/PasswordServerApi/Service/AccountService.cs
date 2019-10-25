﻿using System.Collections.Generic;
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
		private IBaseService _baseService;
		private ILoggingService _logger;
		public AccountService(IBaseService baseService, ILoggingService logger)
		{
			_baseService = baseService;
			_logger = logger;
		}

		#region Dictionary ActionId To Function

		private Dictionary<Guid, Func<AccountDto, AccountDto, AccountActionRequest, string, AccountActionResponse>> _actionIdToFunction = null;

		private Dictionary<Guid, Func<AccountDto, AccountDto, AccountActionRequest, string, AccountActionResponse>> ActionIdToFunction
		{
			get
			{
				return _actionIdToFunction ?? (_actionIdToFunction =
					new Dictionary<Guid, Func<AccountDto, AccountDto, AccountActionRequest, string, AccountActionResponse>>()
					{
						{ StaticConfiguration.ActionSaveAccountId, ActionSeveAccountFunc },
						{ StaticConfiguration.ActionGetAccountId, ActionGetAccountFunc },
						//{ StaticConfiguration.ActionAddNewAccountId, ActionAddNewAccountFunc },
						{ StaticConfiguration.ActionGetAccountAndPasswordId, ActionGetAccountAndPasswordFunc },
						{ StaticConfiguration.ActionRemoveAccountId, ActionRemoveAccountFunc },

					});
			}
		}

		#endregion

		public Response<AccountActionResponse> ExecuteAction(AccountActionRequest request, Guid userID)
		{
			AccountDto userAccount = _baseService.GetAccountById(userID, false);
			if (request.AccountId == null || request.AccountId == Guid.Empty)
				request.AccountId = userID;
			AccountDto savedAccount = _baseService.GetAccountById(request.AccountId, false);

			if (request.Account == null)
				request.Account = savedAccount;

			if (!StaticConfiguration.GetAcrionByRole.ContainsKey(userAccount.Role))
				throw new Exception("Invalid Profile");
			else
			{
				ApplicationAction actions = StaticConfiguration.GetAcrionByRole[userAccount.Role].Find(x => x.Id == request.ActionId);
				if (actions == null)
					throw new Exception("Invalid Action");
				Func<AccountDto, AccountDto, AccountActionRequest, string, AccountActionResponse> func;
				if (!this.ActionIdToFunction.TryGetValue(request.ActionId, out func)) throw new Exception("Δεν βρέθηκε ενέργεια για το Id: " + request.ActionId);

				AccountActionResponse results = func(savedAccount, request.Account, request, userAccount.Role);
				return new Response<AccountActionResponse>()
				{
					Payload = results,
					SelectedAction = request.ActionId,
				};
			}
		}

		#region Actions

		private AccountActionResponse ActionSeveAccountFunc(AccountDto savedAccount, AccountDto requestedAccount, AccountActionRequest request, string Role)
		{
			AccountDto resultAccount = new AccountDto();
			request.Account = resultAccount;
			List<AccountDto> accountsList = _baseService.GetAccounts(request,false).ToList();
			
			//if exist then update
			if (accountsList.Find(x => x.FirstName == requestedAccount.FirstName || x.UserName == requestedAccount.UserName || x.Email == requestedAccount.Email) != null)
				resultAccount = _baseService.UpdateAccount(requestedAccount, Role, false);
			else
			{
				requestedAccount.AccountId = Guid.NewGuid();
				resultAccount = _baseService.AddNewAccount(requestedAccount);
			}

			return new AccountActionResponse() { Accounts = new List<AccountDto>() { resultAccount } };
		}

		private AccountActionResponse ActionGetAccountFunc(AccountDto savedAccount, AccountDto requestedAccount, AccountActionRequest request, string Role)
		{
			List<PasswordDto> passwords = new List<PasswordDto>();
			savedAccount.Passwords.ForEach(x => passwords.Add(_baseService.GetPassword(x.PasswordId)));
			savedAccount.Passwords = passwords;
			return new AccountActionResponse() { Accounts = new List<AccountDto>() { savedAccount } };
		}

		//private AccountActionResponse ActionAddNewAccountFunc(AccountDto savedAccount, AccountDto requestedAccount, AccountActionRequest request, string Role)
		//{
		//	requestedAccount.AccountId = Guid.NewGuid();
		//	requestedAccount.Passwords = new List<PasswordDto>();

		//	if (_baseService.GetAccounts(new AccountActionRequest() { Account = new AccountDto() { UserName = requestedAccount?.UserName } }, false).ToList().Count > 0)
		//		throw new Exception("Rong Username");

		//	_baseService.AddNewAccount(requestedAccount);

		//	return new AccountActionResponse() { Accounts = new List<AccountDto>() { requestedAccount } };
		//}

		private AccountActionResponse ActionGetAccountAndPasswordFunc(AccountDto savedAccount, AccountDto requestedAccount, AccountActionRequest request, string Role)
		{

			return new AccountActionResponse() { Accounts = _baseService.GetAccounts(request, false).ToList() };
		}

		private AccountActionResponse ActionRemoveAccountFunc(AccountDto savedAccount, AccountDto requestedAccount, AccountActionRequest request, string Role)
		{
			AccountDto accountToDelete = _baseService.GetAccountById(requestedAccount.AccountId, false);
			accountToDelete.Passwords.ForEach(password =>
			{
				password = _baseService.GetPassword(password.PasswordId);
				_baseService.RemovePassword(password);
			});
			_baseService.RemoveAccount(accountToDelete);

			return new AccountActionResponse() { Accounts = _baseService.GetAccounts(request, false).ToList() };
		}
		#endregion

	}
}
