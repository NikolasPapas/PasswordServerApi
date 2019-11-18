﻿using PasswordServerApi.Databases.DataModels;
using PasswordServerApi.DataSqliteDB;
using System;
using System.Collections.Generic;

namespace PasswordServerApi.DataFileDb
{
	public interface IReadFileDb
	{
		IEnumerable<EndityAbstractModelAccount> ReadAccountFile();

		IEnumerable<EndityAbstractModelPassword> ReadPasswordFile();

		IEnumerable<LoginTokenModel> ReadFileTokenToken();

		IEnumerable<EndityAbstractModelAccount> WriteAccountFile(List<EndityAbstractModelAccount> accountModel);

		IEnumerable<EndityAbstractModelPassword> WritePasswordFile(List<EndityAbstractModelPassword> passwordModel);

		IEnumerable<LoginTokenModel> WriteFileToken(List<LoginTokenModel> loginTokens);
	}
}