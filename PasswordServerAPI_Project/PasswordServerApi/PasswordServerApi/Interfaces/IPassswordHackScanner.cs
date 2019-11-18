using PasswordServerApi.Models.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.Interfaces
{
	public interface IPassswordHackScanner
	{
		Task<IsHackedResponce> IsThisEmailHacked(string email);

		Task<HackTimeResponce> EmailHackedInfo(string email);

	}
}
