using PasswordServerApi.DataSqliteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.DataFileDb
{
	public interface IApplicationFileDb
	{

		IEnumerable<EndityAbstractModelAccount> ReadDbAccountFile();

		IEnumerable<EndityAbstractModelAccount> SaveToAccountFile(EndityAbstractModelAccount data);

		IEnumerable<EndityAbstractModelAccount> DeleteToAccountFile(EndityAbstractModelAccount data);



		IEnumerable<EndityAbstractModelPassword> ReadDbPasswordFile();

		IEnumerable<EndityAbstractModelPassword> SaveToPasswordFile(EndityAbstractModelPassword data);

		IEnumerable<EndityAbstractModelPassword> DeleteToPasswordFile(EndityAbstractModelPassword data);
	}
}
