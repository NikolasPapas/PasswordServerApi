using PasswordServerApi.DataSqliteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.DataFileDb
{
	public class ReadFileDb : IReadFileDb
	{
		private string IMPORT_PATH = "";
		private string IMPORT_FILENAME_ACCOUNT = "";
		private string IMPORT_FILENAME_PASSWORD = "";
		private readonly char[] FieldDelimiter = new char[] { ';' };
		private readonly char[] PasswordIdsDelimiter = new char[] { ';' };

		public ReadFileDb(string import_FilePath, string import_FileName_account, string import_FileName_password)
		{
			IMPORT_PATH = import_FilePath;
			IMPORT_FILENAME_ACCOUNT = import_FileName_account;
			IMPORT_FILENAME_PASSWORD = import_FileName_password;

		}

		private IEnumerable<string> FindFile(string path,string name)
		{
			string[] files = Directory.GetFiles(path, name);
			if (files != null && files.Length > 0)
				return files.ToList();
			return null;
		}

		public IEnumerable<EndityAbstractModelAccount> ReadAccountFile()
		{
			List<EndityAbstractModelAccount> accounts = new List<EndityAbstractModelAccount>();
			string filename = FindFile(IMPORT_PATH, IMPORT_FILENAME_ACCOUNT).First();
			List<string[]> fileRows = ReadDocument(filename);
			if (fileRows == null || fileRows.Count < 1)
				throw new Exception($"Error on File parse. Name: {filename} , Lines: {fileRows.Count}.");

			for (int line = 0; line < fileRows.Count; line++)
				accounts.Add(GetAccountFromFile(fileRows[line]));
			return accounts;
		}

		public IEnumerable<EndityAbstractModelPassword> ReadPasswordFile()
		{
			List<EndityAbstractModelPassword> passwords = new List<EndityAbstractModelPassword>();
			string filename = FindFile(IMPORT_PATH, IMPORT_FILENAME_PASSWORD).First();
			List<string[]> fileRows = ReadDocument(filename);
			if (fileRows == null || fileRows.Count < 1)
				throw new Exception($"Error on File parse. Name: {filename} , Lines: {fileRows.Count}.");

			for (int line = 0; line < fileRows.Count; line++)
				passwords.Add(GetPasswordFromFile(fileRows[line]));
			return passwords;
		}

		private List<string[]> ReadDocument(string path)
		{
			string[] lines = File.ReadAllLines(path);
			List<string[]> fileRows = new List<string[]>();
			foreach (string line in lines)
			{
				fileRows.Add(line.Split(FieldDelimiter, StringSplitOptions.RemoveEmptyEntries));
			}
			return fileRows;
		}

		private EndityAbstractModelAccount GetAccountFromFile(string[] row)
		{
			return new EndityAbstractModelAccount()
			{
				EndityId = row[0],
				JsonData = row[1],
			};
		}

		private EndityAbstractModelPassword GetPasswordFromFile(string[] row)
		{
			return new EndityAbstractModelPassword
			{
				EndityId = row[0],
				JsonData = row[1],
			};
		}

	}

}
