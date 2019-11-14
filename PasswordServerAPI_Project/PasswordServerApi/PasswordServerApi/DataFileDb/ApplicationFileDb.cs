using Microsoft.EntityFrameworkCore;
using PasswordServerApi.DataSqliteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.DataFileDb
{
	public class ApplicationFileDb : IApplicationFileDb
	{
		public IReadFileDb _fileReader;

		public ApplicationFileDb( IReadFileDb fileReader)
		{
			_fileReader = fileReader;
		}


		#region Account
		private IEnumerable<EndityAbstractModelAccount> Accounts { get; set; }


		public IEnumerable<EndityAbstractModelAccount> ReadDbAccountFile()
		{
			return _fileReader.ReadAccountFile();

		}

		public IEnumerable<EndityAbstractModelAccount> SaveToAccountFile(EndityAbstractModelAccount data)
		{
			List<EndityAbstractModelAccount> accountsModels = ReadDbAccountFile()?.ToList();
			EndityAbstractModelAccount accountModel = accountsModels?.Find(x => x.EndityId == data.EndityId);
			if (accountModel != null)
			{
				int index = accountsModels.FindIndex(x => x.EndityId == data.EndityId);
				accountsModels.RemoveAt(index);
				
			}
			accountsModels.Add(data);

			return _fileReader.WriteAccountFile(accountsModels);
		}


		public IEnumerable<EndityAbstractModelAccount> DeleteToAccountFile(EndityAbstractModelAccount data)
		{
			List<EndityAbstractModelAccount> accountsModels = ReadDbAccountFile()?.ToList();
			EndityAbstractModelAccount accountModel = accountsModels?.Find(x => x.EndityId == data.EndityId);
			if (accountModel != null)
			{
				int index = accountsModels.FindIndex(x => x.EndityId == data.EndityId);
				accountsModels.RemoveAt(index);
			}

			return _fileReader.WriteAccountFile(accountsModels);
		}



		#endregion

		#region Password 

		public IEnumerable<EndityAbstractModelPassword> Passwords { get; set; }

		public IEnumerable<EndityAbstractModelPassword> ReadDbPasswordFile()
		{
			return _fileReader.ReadPasswordFile();
		}

		public IEnumerable<EndityAbstractModelPassword> SaveToPasswordFile(EndityAbstractModelPassword data)
		{
			List<EndityAbstractModelPassword> passwordsModels = ReadDbPasswordFile()?.ToList();
			EndityAbstractModelPassword passwordModel = passwordsModels?.Find(x => x.EndityId == data.EndityId);
			if (passwordModel != null)
			{
				int index = passwordsModels.FindIndex(x => x.EndityId == data.EndityId);
				passwordsModels.RemoveAt(index);
			}
			passwordsModels.Add(data);
			return _fileReader.WritePasswordFile(passwordsModels);
		}

		public IEnumerable<EndityAbstractModelPassword> DeleteToPasswordFile(EndityAbstractModelPassword data)
		{
			List<EndityAbstractModelPassword> passwordsModels = ReadDbPasswordFile()?.ToList();
			EndityAbstractModelPassword passwordModel = passwordsModels?.Find(x => x.EndityId == data.EndityId);
			if (passwordModel != null)
			{
				int index = passwordsModels.FindIndex(x => x.EndityId == data.EndityId);
				passwordsModels.RemoveAt(index);
			}
			return _fileReader.WritePasswordFile(passwordsModels);
		}

		#endregion

	}
}
