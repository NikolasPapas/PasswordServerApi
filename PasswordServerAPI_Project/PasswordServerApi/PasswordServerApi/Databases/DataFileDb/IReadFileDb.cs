using PasswordServerApi.DataSqliteDB;
using System.Collections.Generic;

namespace PasswordServerApi.DataFileDb
{
	public interface IReadFileDb
	{
		IEnumerable<EndityAbstractModelAccount> ReadAccountFile();

		IEnumerable<EndityAbstractModelPassword> ReadPasswordFile();

		IEnumerable<EndityAbstractModelAccount> WriteAccountFile(List<EndityAbstractModelAccount> accountModel);

		IEnumerable<EndityAbstractModelPassword> WritePasswordFile(List<EndityAbstractModelPassword> passwordModel);

	}
}
