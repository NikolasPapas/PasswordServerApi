﻿using PasswordServerApi.DTO;
using PasswordServerApi.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.StorageLayer
{
	public interface IStorageService
	{
		#region Account

		bool ValidateAccount(AccountDto addAccount);

		List<AccountDto> GetAccounts();

		AccountDto SetAccount(AccountDto addAccount, string password);

		void DeleteAccount(AccountDto addAccount);

		#endregion

		#region Password

		List<PasswordDto> GetPasswords();

		PasswordDto SetPassword(PasswordDto addPassword);

		void DeletePassword(PasswordDto addPassword);

		#endregion

		#region Tokens

		List<LoginTokenDto> GetTokens();

		List<LoginTokenDto> SetToken(LoginTokenDto loginToken);

		void DeleteToken(LoginTokenDto loginToken);

		#endregion

		#region Notes

		List<NoteDto> GetNotes();

		List<NoteDto> SetNote(NoteDto note);

		void DeleteNote(NoteDto note);

		#endregion
	}
}
