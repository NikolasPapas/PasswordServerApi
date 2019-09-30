using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PasswordServerApi.DataSqliteDB;
using PasswordServerApi.DTO;
using PasswordServerApi.Interfaces;
using PasswordServerApi.DataSqliteDB.DataModels;
using PasswordServerApi.Models.Responces;
using PasswordServerApi.Models.Requests.Password;
using PasswordServerApi.Models;

namespace PasswordServerApi.Service
{
	public class PasswordService : IPasswordService
	{
		IBaseService _baseService;
		public PasswordService(IBaseService baseService)
		{
			_baseService = baseService;
		}

		#region Dictionary ActionId To Function

		private readonly Dictionary<Guid, Func<PasswordDto, PasswordDto, AccountDto, PasswordActionRequest, Response<List<PasswordDto>>>> _actionIdToFunction;

		private Dictionary<Guid, Func<PasswordDto, PasswordDto, AccountDto, PasswordActionRequest, Response<List<PasswordDto>>>> ActionIdToFunction
		{
			get
			{
				return _actionIdToFunction ??
					new Dictionary<Guid, Func<PasswordDto, PasswordDto, AccountDto, PasswordActionRequest, Response<List<PasswordDto>>>>()
					{
						{ StaticConfiguration.ActionGetPasswordsId, GetPaswordsFunc },
						{ StaticConfiguration.ActionUpdateOrAddPasswordId, UpdateOrAddPasswordFunc },
					};
			}
		}

		#endregion

		public Response<List<PasswordDto>> PasswordAction(PasswordActionRequest request)
		{
			AccountDto account = _baseService.GetAccountById(request.AccountId);
			PasswordDto savedPassword = _baseService.GetPasswords(request, account).FirstOrDefault();

			if (!StaticConfiguration.GetAcrionByRole.ContainsKey(account.Role))
				throw new Exception("Invalid Profile");
			else
			{
				ApplicationAction actions = StaticConfiguration.GetAcrionByRole[account.Role].Find(x => x.Id == request.ActionId);
				if (actions == null)
					throw new Exception("Invalid Action");
				Func<PasswordDto, PasswordDto, AccountDto, PasswordActionRequest, Response<List<PasswordDto>>> func;
				if (!this.ActionIdToFunction.TryGetValue(request.ActionId, out func)) throw new Exception("Δεν βρέθηκε ενέργεια για το Id: " + request.ActionId);
				Response<List<PasswordDto>> response = func(savedPassword, request.Password, account, request);
				response.SelectedAction = request.AccountId;
				return response;
			}

		}

		#region Actions 

		private Response<List<PasswordDto>> GetPaswordsFunc(PasswordDto savedPass, PasswordDto requesPass, AccountDto account, PasswordActionRequest request)
		{
			return new Response<List<PasswordDto>>()
			{
				Payload = _baseService.GetPasswords(request, account).ToList()
			};

		}

		private Response<List<PasswordDto>> UpdateOrAddPasswordFunc(PasswordDto savedPass, PasswordDto requesPass, AccountDto account, PasswordActionRequest request)
		{
			if (account.Passwords.Find(pass =>
			 {
				 bool passExist = false;
				 passExist |= requesPass?.PasswordId == pass.PasswordId;
				 passExist |= requesPass?.UserName == pass.UserName && requesPass?.LogInLink == pass.LogInLink;
				 return passExist;
			 }) != null)
			{
				_baseService.UpdatePassword(requesPass);
			}
			else
			{
				Guid newPassId = Guid.NewGuid();
				requesPass.PasswordId = newPassId;
				account.Passwords.Add(requesPass);
				_baseService.UpdateAccount(account, true);
				_baseService.AddNewPassword(requesPass);
			}

			return new Response<List<PasswordDto>>()
			{
				Payload = new List<PasswordDto>() { requesPass }
			};
		}

		#endregion

	}
}
