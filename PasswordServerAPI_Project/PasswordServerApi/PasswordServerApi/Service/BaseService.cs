﻿using Newtonsoft.Json;
using PasswordServerApi.DataSqliteDB;
using PasswordServerApi.DataSqliteDB.DataModels;
using PasswordServerApi.DTO;
using PasswordServerApi.Interfaces;
using PasswordServerApi.Models.Account.Requests;
using PasswordServerApi.Models.DTO;
using PasswordServerApi.Models.Requests.Password;
using PasswordServerApi.StorageLayer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PasswordServerApi.Service
{
	public class BaseService : IBaseService
	{
		private IStorageService _storageService;

		private ILoggingService _logger;

		public BaseService(IStorageService storageService, ILoggingService logger)
		{
			_storageService = storageService;
			_logger = logger;
		}

		#region Database Connections Account

		public AccountDto GetSpesificAccount(AccountActionRequest request)
		{
			List<AccountDto> accounts = _storageService.GetAccounts();

			return accounts.Find(x => request?.Account?.UserName == x.UserName && request?.Account?.Password == x.Password);
		}

		public IEnumerable<AccountDto> GetAccounts(AccountActionRequest request, bool full)
		{
			List<AccountDto> accounts = _storageService.GetAccounts();
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
			if (full)
				return filtered;
			return filtered.Select(x => { if (request?.Account?.Password == null) { x.Password = ""; /*x.CurrentToken = "";*/ } return x; });
		}

		public AccountDto UpdateAccount(AccountDto accountDto, string Role, bool full = false)
		{
			AccountDto accountToApdate = _storageService.GetAccounts().Find(x => x.AccountId == accountDto.AccountId);

			if (Role != "Admin" && accountDto.Password != accountToApdate.Password)
				throw new Exception("Invalid Password");

			accountToApdate.Email = accountDto.Email;
			accountToApdate.FirstName = accountDto.FirstName;
			accountToApdate.LastName = accountDto.LastName;
			accountToApdate.Sex = accountDto.Sex;

			if (full)
			{
				accountToApdate.LastLogIn = accountDto.LastLogIn;
				accountToApdate.Role = accountDto.Role;
				//accountToApdate.CurrentToken = accountDto.CurrentToken;
				accountToApdate.Password = accountDto.Password;
				accountToApdate.Passwords = accountDto.Passwords;
			}

			_storageService.SetAccount(accountToApdate);

			return accountDto;
		}

		public AccountDto GetAccountById(Guid id, bool full)
		{
			AccountDto results = _storageService.GetAccounts().Find(x => x.AccountId == id);
			if (!full)
				results.Password = "";
			return results;
		}

		public AccountDto AddNewAccount(AccountDto request)
		{
			return _storageService.SetAccount(request);
		}

		public AccountDto RemoveAccount(AccountDto request)
		{
			var accountToRemove = _storageService.GetAccounts().Find(x => x.AccountId == request.AccountId);
			_storageService.DeleteAccount(accountToRemove);
			return accountToRemove;
		}

		#endregion

		#region Database Connections Passwords

		public IEnumerable<PasswordDto> GetPasswords(PasswordActionRequest request, AccountDto account)
		{
			List<PasswordDto> passwords = new List<PasswordDto>();

			_storageService.GetPasswords().ForEach(x =>
			{
				if (account.Passwords.Find(accountPass => accountPass.PasswordId == x.PasswordId) == null)
					return;
				bool haseCorrectValues = false;
				haseCorrectValues = (!string.IsNullOrWhiteSpace(request?.Password?.Name) ? request?.Password?.Name == x?.Name : true) && (!string.IsNullOrWhiteSpace(request?.Password?.LogInLink) ? request?.Password?.LogInLink == x?.LogInLink : true) && (!string.IsNullOrWhiteSpace(request?.Password?.UserName) ? request?.Password?.LogInLink == x?.LogInLink : true);
				if (haseCorrectValues)
					passwords.Add(x);
			});
			return passwords;
		}

		public PasswordDto GetPassword(Guid id)
		{
			return _storageService.GetPasswords().Find(x => x.PasswordId == id);
		}

		public PasswordDto UpdatePassword(PasswordDto passwordDto)
		{
			var passwordModelData = _storageService.GetPasswords().Find(x => x.PasswordId == passwordDto.PasswordId);
			return _storageService.SetPassword(passwordDto);
		}

		public PasswordDto AddNewPassword(PasswordDto requestPassword)
		{
			_storageService.SetPassword(requestPassword);
			return requestPassword;
		}

		public PasswordDto RemovePassword(PasswordDto requestPassword)
		{
			_storageService.DeletePassword(requestPassword);
			return requestPassword;
		}

		#region Fill Database

		public void FilldDatabase(List<AccountDto> accounts)
		{
			foreach (AccountDto account in accounts)
			{
				_storageService.SetAccount(account);
				account.Passwords.ForEach(pass => _storageService.SetPassword(pass));
			}
		}

		#endregion

		#endregion

		#region Database Connections Tokens

		public List<LoginTokenDto> FindUserTokens(Guid id)
		{
			return _storageService.GetTokens().FindAll(x => x.UserId == id);
		}

		public LoginTokenDto FindToken(Guid id, string Token)
		{
			return _storageService.GetTokens().Find(x => x.Token == Token && x.UserId == id);
		}

		public LoginTokenDto SaveToken(Guid id, string userAgent, string Token)
		{
			_storageService.SetToken(new LoginTokenDto() { LoginTokenId = Guid.NewGuid(), UserId = id, UserAgent = userAgent, Token = Token });

			return FindToken(id, Token);
		}

		public void DeleteToken(Guid id, string userAgent, string Token)
		{
			List<LoginTokenDto> tokensToDelete = _storageService.GetTokens().FindAll(x => x.UserId == id && (x.Token == Token || x.UserAgent == userAgent));

			tokensToDelete.ForEach(token =>
			{
				_storageService.DeleteToken(token);
			});
		}

		#endregion

	}
}
