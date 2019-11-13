using PasswordServerApi.DataSqliteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.DataFileDb
{
	public interface IReadFileDb
	{

		IEnumerable<EndityAbstractModelAccount> ReadAccountFile();

		IEnumerable<EndityAbstractModelPassword> ReadPasswordFile();

	}
}
