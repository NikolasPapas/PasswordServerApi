using System;
using System.Collections.Generic;
using System.Linq;
using PasswordServerApi.DTO;
using PasswordServerApi.Interfaces;
using PasswordServerApi.Models.Responces;
using PasswordServerApi.Models.Requests.Password;
using PasswordServerApi.Models;

namespace PasswordServerApi.Service
{
	public class PasswordService : IPasswordService
	{
		private IBaseService _baseService;
		private ILoggingService _logger;
		public PasswordService(IBaseService baseService, ILoggingService logger)
		{
			_baseService = baseService;
			_logger = logger;
		}

		#region Dictionary ActionId To Function

		private Dictionary<Guid, Func<PasswordDto, PasswordDto, AccountDto, PasswordActionRequest, PasswordActionResponse>> _actionIdToFunction = null;

		private Dictionary<Guid, Func<PasswordDto, PasswordDto, AccountDto, PasswordActionRequest, PasswordActionResponse>> ActionIdToFunction
		{
			get
			{
				return _actionIdToFunction ?? (_actionIdToFunction =
					new Dictionary<Guid, Func<PasswordDto, PasswordDto, AccountDto, PasswordActionRequest, PasswordActionResponse>>()
					{
						{ StaticConfiguration.ActionGetPasswordsId, GetPaswordsFunc },
						{ StaticConfiguration.ActionUpdateOrAddPasswordId, UpdateOrAddPasswordFunc },
						{ StaticConfiguration.ActionRemovePasswordId, RemovePasswordFunc },
					});
			}
		}

		#endregion

		public Response<PasswordActionResponse> PasswordAction(PasswordActionRequest request)
		{
			AccountDto account = _baseService.GetAccountById(request.AccountId,false);
			PasswordDto savedPassword = _baseService.GetPasswords(request, account).FirstOrDefault();

			if (!StaticConfiguration.GetAcrionByRole.ContainsKey(account.Role))
				throw new Exception("Invalid Profile");
			else
			{
				ApplicationAction actions = StaticConfiguration.GetAcrionByRole[account.Role].Find(x => x.Id == request.ActionId);
				if (actions == null)
					throw new Exception("Invalid Action");
				Func<PasswordDto, PasswordDto, AccountDto, PasswordActionRequest, PasswordActionResponse> func;
				if (!this.ActionIdToFunction.TryGetValue(request.ActionId, out func)) throw new Exception("Δεν βρέθηκε ενέργεια για το Id: " + request.ActionId);
				PasswordActionResponse response = func(savedPassword, request.Password, account, request);
				return new Response<PasswordActionResponse>() {Payload= response, SelectedAction = request.AccountId };
			}

		}

		#region Actions 

		private PasswordActionResponse GetPaswordsFunc(PasswordDto savedPass, PasswordDto requesPass, AccountDto account, PasswordActionRequest request)
		{
			return new PasswordActionResponse() { Passwords = _baseService.GetPasswords(request, account).ToList() };

		}

		private PasswordActionResponse UpdateOrAddPasswordFunc(PasswordDto savedPass, PasswordDto requesPass, AccountDto account, PasswordActionRequest request)
		{
			if (account.Passwords.Find(pass => requesPass?.PasswordId == pass.PasswordId) != null)
				_baseService.UpdatePassword(requesPass);
			else
			{
				Guid newPassId = Guid.NewGuid();
				requesPass.PasswordId = newPassId;
				account.Passwords.Add(requesPass);
				_baseService.UpdateAccount(account, true);
				_baseService.AddNewPassword(requesPass);
			}

			return new PasswordActionResponse() { Passwords = new List<PasswordDto>() { requesPass } };
		}


		private PasswordActionResponse RemovePasswordFunc(PasswordDto savedPass, PasswordDto requesPass, AccountDto account, PasswordActionRequest request)
		{
			int index = account.Passwords.FindIndex(pass => requesPass?.PasswordId == pass.PasswordId);
			if (index < 0 && index > account.Passwords.Count())
				throw new Exception("invalid PasswordId");

			account.Password.Remove(index);
			_baseService.UpdateAccount(account, true);
			_baseService.RemovePassword(requesPass);

			return new PasswordActionResponse() { Passwords = new List<PasswordDto>() { requesPass } };
		}



		#endregion

	}
}
