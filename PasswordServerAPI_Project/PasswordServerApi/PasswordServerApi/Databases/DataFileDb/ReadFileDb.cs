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

		private IEnumerable<string> FindFile(string path, string name)
		{
			string[] files = Directory.GetFiles(path, name);
			if (files != null && files.Length > 0)
				return files.ToList();
			return null;
		}

		#region Read

		public IEnumerable<EndityAbstractModelAccount> ReadAccountFile()
		{
			List<EndityAbstractModelAccount> accounts = new List<EndityAbstractModelAccount>();
			string filename = FindFile(IMPORT_PATH, IMPORT_FILENAME_ACCOUNT).First();
			List<string[]> fileRows = ReadDocument(filename);
			if (fileRows == null || fileRows.Count < 1)
			{
				return new List<EndityAbstractModelAccount>();
				throw new Exception($"Error on File parse. Name: {filename} , Lines: {fileRows.Count}.");
			}

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
			{
				return new List<EndityAbstractModelPassword>();
				throw new Exception($"Error on File parse. Name: {filename} , Lines: {fileRows.Count}.");
			}

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

		#endregion

		#region Write

		public IEnumerable<EndityAbstractModelAccount> WriteAccountFile(List<EndityAbstractModelAccount> accountModel)
		{
			List<string> fileRowsTowrite = new List<string>();
			accountModel.ForEach(x => fileRowsTowrite.Add(SetAccountFromModel(x)));
			string filename = FindFile(IMPORT_PATH, IMPORT_FILENAME_ACCOUNT).First();
			WriteDocument(filename, fileRowsTowrite);
			return ReadAccountFile();
		}


		public IEnumerable<EndityAbstractModelPassword> WritePasswordFile(List<EndityAbstractModelPassword> passwordModel)
		{

			List<string> fileRowsTowrite = new List<string>();
			passwordModel.ForEach(x => fileRowsTowrite.Add(SetPasswordFromModel(x)));
			string filename = FindFile(IMPORT_PATH, IMPORT_FILENAME_PASSWORD).First();
			WriteDocument(filename, fileRowsTowrite);
			return ReadPasswordFile();
		}


		private void WriteDocument(string path, List<string> fileRows)
		{
			string[] lines = new string[fileRows.Count()];

			for (int i = 0; i < fileRows.Count(); i++)
			{
				string line = fileRows.ElementAt(i);
				lines[i] = line;
			}
			if (!File.Exists(path))
				throw new Exception("file Not Exist");
			File.WriteAllLines(path, lines);
		}

		#endregion

		#region Helpers 

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

		private string SetAccountFromModel(EndityAbstractModelAccount account)
		{
			if (account != null)
				return account.EndityId + FieldDelimiter[0] + account.JsonData;
			return "";
		}

		private string SetPasswordFromModel(EndityAbstractModelPassword password)
		{
			if (password != null)
				return password.EndityId + FieldDelimiter[0] + password.JsonData;
			return "";
		}

		#endregion

	}

}
