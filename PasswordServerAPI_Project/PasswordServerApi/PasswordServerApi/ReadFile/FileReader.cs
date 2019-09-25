using PasswordServerApi.Models.FileModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.ReadFile
{
	public class FileReader
	{
		private const string IMPORT_PATH_LOG_IN = "";
		private const string IMPORT_FILENAME_LOG_IN = "";
		private readonly char[] FieldDelimiter = new char[] { ';' };
		private readonly char[] PasswordIdsDelimiter = new char[] { '_' };

		public FileReader()
		{

		}

		private IEnumerable<string> FindFileForLogIn()
		{
			string[] files = Directory.GetFiles(IMPORT_PATH_LOG_IN, IMPORT_FILENAME_LOG_IN);
			if (files != null && files.Length > 0)
				return files.ToList();
			return null;
		}

		private IEnumerable<FileAccountModel> ReadLognFile(string filename)
		{
			List<FileAccountModel> accounts = new List<FileAccountModel>();

			List<string[]> fileRows = ReadDocument(filename);
			if (fileRows == null || fileRows.Count < 1)
				throw new Exception($"Error on File parse. Name: {filename} , Lines: {fileRows.Count}.");

			for (int line = 0; line < fileRows.Count; line++)
				accounts.Add(GetAccountFromFile(fileRows[line]));
			return accounts;
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

		private FileAccountModel GetAccountFromFile(string[] row)
		{
			return new FileAccountModel()
			{
				AccountId = row[0],
				FirstName = row[1],
				LastName = row[2],
				UserName = row[3],
				Email = row[4],
				Sex = row[5],
				LastLogIn = row[6],
				Password = row[7],
				PasswordsIds = row[8].Split(PasswordIdsDelimiter, StringSplitOptions.RemoveEmptyEntries).ToList()
			};
		}

	}
}
