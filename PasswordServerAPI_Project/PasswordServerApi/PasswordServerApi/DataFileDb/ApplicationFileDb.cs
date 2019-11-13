using Microsoft.EntityFrameworkCore;
using PasswordServerApi.DataSqliteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.DataFileDb
{
	public class ApplicationFileDb
	{
		public IReadFileDb _fileReader;

		public ApplicationFileDb( IReadFileDb fileReader)
		{
			_fileReader = fileReader;
		}

		public IEnumerable<EndityAbstractModelAccount> Accounts
		{
			get
			{
				return ReadDbAccountFile();
			}
			set
			{
				//(data) => SaveToAccountFile(data);
			}
		}


		private IEnumerable<EndityAbstractModelAccount> ReadDbAccountFile()
		{
			return _fileReader.ReadAccountFile();

		}

		private void SaveToAccountFile(IEnumerable<EndityAbstractModelAccount> data)
		{

		}


		public IEnumerable<EndityAbstractModelPassword> Passwords
		{
			get
			{
				return ReadDbPasswordFile();
			}
			set
			{
			}
		}

		private IEnumerable<EndityAbstractModelPassword> ReadDbPasswordFile()
		{
			return _fileReader.ReadPasswordFile();
		}

		private void SaveToPasswordFile(IEnumerable<EndityAbstractModelPassword> data)
		{

		}

	}
}
