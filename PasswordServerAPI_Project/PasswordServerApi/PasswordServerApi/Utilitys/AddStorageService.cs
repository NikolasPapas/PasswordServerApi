using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PasswordServerApi.DataFileDb;
using PasswordServerApi.DataSqliteDB;
using PasswordServerApi.StorageLayer;
using PasswordServerApi.Utilitys.Configuration;

namespace PasswordServerApi.Utilitys
{
	public static class AddStorageService
	{

		public static void AddFilleDB(this IServiceCollection services, IConfigurationManager configurationManager )
		{
			
			services.AddTransient<IReadFileDb>(s => new ReadFileDb(
				configurationManager.GetString("FileDB", "MainPath"),
				configurationManager.GetString("FileDB", "AccountsPath"),
				configurationManager.GetString("FileDB", "PasswordPath"),
				configurationManager.GetString("FileDB", "LoginTokensPath")));
			services.AddTransient<IStorageService, StorageServiceFile>();
		}

		public static void AddSQLLiteDb(this IServiceCollection services, IConfigurationManager configurationManager)
		{
			services.AddEntityFrameworkSqlServer().AddEntityFrameworkSqlite().AddDbContext<ApplicationDbContext>(options => options.UseSqlite($"Data Source={configurationManager.GetString("sqliteDB", "MainPath")}"));
			services.AddTransient<IStorageService, StorageServiceDb>();
		}
	}
}
