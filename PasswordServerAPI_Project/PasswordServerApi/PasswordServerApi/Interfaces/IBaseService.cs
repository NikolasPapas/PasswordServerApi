using PasswordServerApi.DTO;
using PasswordServerApi.Models.Account.Requests;
using PasswordServerApi.Models.DTO;
using PasswordServerApi.Models.Requests.Password;
using System;
using System.Collections.Generic;

namespace PasswordServerApi.Interfaces
{
	public interface IBaseService
	{

		#region Account

		AccountDto GetSpesificAccount(AccountActionRequest request);

		IEnumerable<AccountDto> GetAccounts(AccountActionRequest request);

		AccountDto UpdateAccount(AccountDto accountDto, string Role, bool full);

		AccountDto GetAccountById(Guid id, bool full);

		AccountDto AddNewAccount(AccountDto request);

		AccountDto RemoveAccount(AccountDto request);

		#endregion

		#region Pasword 

		PasswordDto GetPassword(Guid id);

		PasswordDto UpdatePassword(PasswordDto passwordDto);

		IEnumerable<PasswordDto> GetPasswords(PasswordActionRequest request, AccountDto account);

		PasswordDto AddNewPassword(PasswordDto requestPassword);

		PasswordDto RemovePassword(PasswordDto requestPassword);

		#endregion

		#region Tokens

		List<LoginTokenDto> FindUserTokens(Guid id);

		LoginTokenDto FindToken(Guid id, string Token);

		LoginTokenDto SaveToken(Guid id, string userAgent, string Token);

		void DeleteToken(Guid id, string userAgent, string Token);

		#endregion

		#region Notes

		List<NoteDto> FindUserNotes(Guid id);

		NoteDto FindNote(Guid id, string note);

		NoteDto SaveNote(Guid id, Guid noteId, string note);

		void DeleteNote(Guid id, Guid noteId);

		#endregion

		void FilldDatabase(List<AccountDto> accounts);

	}
}
