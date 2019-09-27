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

		private readonly Dictionary<Guid, Func<PasswordDto, PasswordDto, Response<List<PasswordDto>>>> _actionIdToFunction;

		private Dictionary<Guid, Func<PasswordDto, PasswordDto, Response<List<PasswordDto>>>> ActionIdToFunction
		{
			get
			{
				return _actionIdToFunction ??
					new Dictionary<Guid, Func<PasswordDto, PasswordDto, Response<List<PasswordDto>>>>()
					{
						{ StaticConfiguration.ActionGetPasswordsId, GetPaswordsFunc },
						{ StaticConfiguration.ActionSavePasswordId, SeveAccountFunc },
					};
			}
		}

		#endregion




		public Response<List<PasswordDto>> PasswordAction(PasswordActionRequest request)
		{
			AccountDto account = _baseService.GetAccountById(request.AccountId);
			PasswordDto savedPassword = _baseService.GetPasswords(request).FirstOrDefault();

			if (!StaticConfiguration.GetAcrionByRole.ContainsKey(account.Role))
				throw new Exception("Invalid Profile");
			else
			{
				ApplicationAction actions = StaticConfiguration.GetAcrionByRole[account.Role].Find(x => x.Id == request.ActionId);
				if (actions == null)
					throw new Exception("Invalid Action");
				Func<PasswordDto, PasswordDto, Response<List<PasswordDto>>> func;
				if (!this.ActionIdToFunction.TryGetValue(request.ActionId, out func)) throw new Exception("Δεν βρέθηκε ενέργεια για το Id: " + request.ActionId);
				return func(savedPassword, request.Password);
			}

		}

		#region Actions 

		private Response<List<PasswordDto>> GetPaswordsFunc(PasswordDto savedPass, PasswordDto requesPass)
		{
			return new Response<List<PasswordDto>>()
			{
				Payload = new List<PasswordDto>() { savedPass }
			};

		}


		private Response<List<PasswordDto>> SeveAccountFunc(PasswordDto savedPass, PasswordDto requesPass)
		{
			return new Response<List< PasswordDto >> ()
			{
				Payload = new List<PasswordDto>() { savedPass }
			};
		}

		#endregion

	}
}
