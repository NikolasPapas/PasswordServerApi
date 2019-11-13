using Newtonsoft.Json;
using PasswordServerApi.DataSqliteDB;
using PasswordServerApi.DataSqliteDB.DataModels;
using PasswordServerApi.DTO;
using PasswordServerApi.Interfaces;
using PasswordServerApi.Models.Account.Requests;
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
			List<AccountDto> accounts = _storageService.GetAccountsDto();

			return accounts.Find(x => request?.Account?.UserName == x.UserName && request?.Account?.Password == x.Password);
		}

		public IEnumerable<AccountDto> GetAccounts(AccountActionRequest request, bool full)
		{
			List<AccountDto> accounts = _storageService.GetAccountsDto();
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
			return filtered.Select(x => { if (request?.Account?.Password == null) { x.Password = ""; x.CurrentToken = ""; } return x; });
		}

		public AccountDto UpdateAccount(AccountDto accountDto, string Role, bool full = false)
		{
			AccountDto accountToApdate = _storageService.GetAccountsDto().Find(x => x.AccountId == accountDto.AccountId);

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
				accountToApdate.CurrentToken = accountDto.CurrentToken;
				accountToApdate.Password = accountDto.Password;
				accountToApdate.Passwords = accountDto.Passwords;
			}

			_storageService.SetAccountsDto(accountToApdate);

			return accountDto;
		}

		public AccountDto GetAccountById(Guid id, bool full)
		{
			AccountDto results = _storageService.GetAccountsDto().Find(x => x.AccountId == id);
			if (!full)
				results.Password = "";
			return results;
		}

		public AccountDto AddNewAccount(AccountDto request)
		{
			return _storageService.SetAccountsDto(request);
		}

		public AccountDto RemoveAccount(AccountDto request)
		{
			var accountToRemove = _storageService.GetAccountsDto().Find(x => x.AccountId == request.AccountId);
			_storageService.DeleteAccountsDto(accountToRemove);
			return accountToRemove;
		}

		#endregion

		#region Database Connections Passwords

		public IEnumerable<PasswordDto> GetPasswords(PasswordActionRequest request, AccountDto account)
		{
			List<PasswordDto> passwords = new List<PasswordDto>();

			_storageService.GetPasswordsDto().ForEach(x =>
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
			return _storageService.GetPasswordsDto().Find(x => x.PasswordId == id);
		}

		public PasswordDto UpdatePassword(PasswordDto passwordDto)
		{
			var passwordModelData = _storageService.GetPasswordsDto().Find(x => x.PasswordId == passwordDto.PasswordId);
			return _storageService.SetPasswordsDto(passwordDto);
		}

		public PasswordDto AddNewPassword(PasswordDto requestPassword)
		{
			_storageService.SetPasswordsDto(requestPassword);
			return requestPassword;
		}

		public PasswordDto RemovePassword(PasswordDto requestPassword)
		{
			_storageService.DeletePasswordsDto(requestPassword);
			return requestPassword;
		}

		#region Fill Database

		public void FilldDatabase(List<AccountDto> accounts)
		{
			foreach (AccountDto account in accounts)
			{
				_storageService.SetAccountsDto(account);
				account.Passwords.ForEach(pass => _storageService.SetPasswordsDto(pass));
			}
		}

		#endregion

		#endregion

	}
}
