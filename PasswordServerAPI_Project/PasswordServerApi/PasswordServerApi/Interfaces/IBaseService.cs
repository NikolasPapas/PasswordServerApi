﻿using PasswordServerApi.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.Interfaces
{
    public interface IBaseService
	{
		PasswordDto GetDumyPassword(int i);

		AccountDto GetDumyAccount(int i);

	}
}
