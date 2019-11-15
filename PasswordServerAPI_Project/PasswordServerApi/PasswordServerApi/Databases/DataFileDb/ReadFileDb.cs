using PasswordServerApi.Databases.DataModels;
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
		private string IMPORT_FILENAME_LOGIN_TOKENS = "";
		private readonly char[] AccountFieldDelimiter = new char[] { ';' };
		private readonly char[] PasswordIdsDelimiter = new char[] { ';' };
		private readonly char[] LoginTokenFieldDelimiter = new char[] { '=' };

		public ReadFileDb(string import_FilePath, string import_FileName_account, string import_FileName_password, string import_FileName_login_tokens)
		{
			IMPORT_PATH = import_FilePath;
			IMPORT_FILENAME_ACCOUNT = import_FileName_account;
			IMPORT_FILENAME_PASSWORD = import_FileName_password;
			IMPORT_FILENAME_LOGIN_TOKENS = import_FileName_login_tokens;

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
			try
			{
				List<string[]> fileRows = ReadFromFile(IMPORT_FILENAME_ACCOUNT,AccountFieldDelimiter);
				for (int line = 0; line < fileRows.Count; line++)
					accounts.Add(GetAccountFromFile(fileRows[line]));
				return accounts;
			}
			catch (Exception ex)
			{
				return new List<EndityAbstractModelAccount>();
			}
		}

		public IEnumerable<EndityAbstractModelPassword> ReadPasswordFile()
		{
			List<EndityAbstractModelPassword> passwords = new List<EndityAbstractModelPassword>();
			try
			{
				List<string[]> fileRows = ReadFromFile(IMPORT_FILENAME_PASSWORD,PasswordIdsDelimiter);
				for (int line = 0; line < fileRows.Count; line++)
					passwords.Add(GetPasswordFromFile(fileRows[line]));
				return passwords;
			}
			catch (Exception ex)
			{
				return new List<EndityAbstractModelPassword>();
			}
		}

		public IEnumerable<LoginTokenModel> ReadFileTokenToken()
		{
			List<LoginTokenModel> LoginTokens = new List<LoginTokenModel>();
			try
			{
				List<string[]> fileRows = ReadFromFile(IMPORT_FILENAME_LOGIN_TOKENS,LoginTokenFieldDelimiter);
				for (int line = 0; line < fileRows.Count; line++)
					LoginTokens.Add(GetLoginTokenFromFile(fileRows[line]));
				return LoginTokens;
			}
			catch (Exception ex)
			{
				return new List<LoginTokenModel>();
			}
		}

		private List<string[]> ReadFromFile(string filePath, char[] fileDelimiter)
		{
			string filename = FindFile(IMPORT_PATH, filePath).First();
			List<string[]> fileRows = ReadDocument(filename , fileDelimiter);
			if (fileRows == null || fileRows.Count < 1)
			{
				throw new Exception($"Error on File parse. Name: {filename} , Lines: {fileRows.Count}.");
			}
			return fileRows;
		}

		private List<string[]> ReadDocument(string path, char[] fileDelimiter)
		{
			string[] lines = File.ReadAllLines(path);
			List<string[]> fileRows = new List<string[]>();
			foreach (string line in lines)
			{
				fileRows.Add(line.Split(fileDelimiter, StringSplitOptions.RemoveEmptyEntries));
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

		public IEnumerable<LoginTokenModel> WriteFileToken(List<LoginTokenModel> loginTokens)
		{
			List<string> fileRowsTowrite = new List<string>();
			loginTokens.ForEach(x => fileRowsTowrite.Add(SetLoginTokensFromModel(x)));
			string filename = FindFile(IMPORT_PATH, IMPORT_FILENAME_LOGIN_TOKENS).First();
			WriteDocument(filename, fileRowsTowrite);
			return ReadFileTokenToken();
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

		private LoginTokenModel GetLoginTokenFromFile(string[] row)
		{
			return new LoginTokenModel
			{
				LoginTokenId = row[0],
				UserId = row[1],
				Token = row[2],
				UserAgent = row[3],
				LastLogIn = DateTime.Parse(row[4]),
			};
		}

		private string SetAccountFromModel(EndityAbstractModelAccount account)
		{
			if (account != null)
				return account.EndityId + AccountFieldDelimiter[0] + account.JsonData;
			return "";
		}

		private string SetPasswordFromModel(EndityAbstractModelPassword password)
		{
			if (password != null)
				return password.EndityId + PasswordIdsDelimiter[0] + password.JsonData;
			return "";
		}

		private string SetLoginTokensFromModel(LoginTokenModel LoginToken)
		{
			if (LoginToken != null)
				return LoginToken.LoginTokenId + LoginTokenFieldDelimiter[0] + LoginToken.UserId + LoginTokenFieldDelimiter[0] + LoginToken.Token + LoginTokenFieldDelimiter[0] + LoginToken.UserAgent + LoginTokenFieldDelimiter[0] + LoginToken.LastLogIn.ToString();
			return "";
		}

		#endregion

	}

}
