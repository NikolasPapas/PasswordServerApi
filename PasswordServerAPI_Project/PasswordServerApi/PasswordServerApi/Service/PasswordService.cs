using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PasswordServerApi.DTO;
using PasswordServerApi.Interfaces;

namespace PasswordServerApi.Service
{
	public class PasswordService : IPasswordService
	{
		public PasswordService()
		{

		}

		public IEnumerable<PasswordDto> GetPasswords()
		{
			return new List<PasswordDto>() { };
		}

		public PasswordDto GetPassword()
		{
			return new PasswordDto() { };
		}
	}
}
