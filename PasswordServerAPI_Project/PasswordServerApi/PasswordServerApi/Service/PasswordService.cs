using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PasswordServerApi.DTO;
using PasswordServerApi.Interfaces;
using PasswordServerApi.Models.Enums;

namespace PasswordServerApi.Service
{
	public class PasswordService : IPasswordService
	{
		IBaseService _baseService;

		public PasswordService(IBaseService baseService)
		{
			_baseService = baseService;
		}

		public IEnumerable<PasswordDto> GetPasswords()
		{
			return new List<PasswordDto>() { _baseService.GetDumyPassword(1), _baseService.GetDumyPassword(2), _baseService.GetDumyPassword(3), _baseService.GetDumyPassword(4), _baseService.GetDumyPassword(5), _baseService.GetDumyPassword(105), };
		}

		public PasswordDto GetPassword()
		{
			return _baseService.GetDumyPassword(1);
		}




	}
}
