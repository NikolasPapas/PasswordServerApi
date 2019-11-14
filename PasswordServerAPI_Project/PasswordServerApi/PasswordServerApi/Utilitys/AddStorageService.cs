using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PasswordServerApi.DataFileDb;
using PasswordServerApi.DataSqliteDB;
using PasswordServerApi.StorageLayer;

namespace PasswordServerApi.Utilitys
{
	public static class AddStorageService
	{

		public static void AddFilleDB(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddTransient<IReadFileDb>(s => new ReadFileDb("C:\\PASSWORDSERVERAPI", "Accounts.txt", "Paswords.txt"));
			services.AddTransient<IApplicationFileDb, ApplicationFileDb>();
			services.AddTransient<IStorageService, StorageServiceFile>();
		}

		public static void AddSQLLiteDb(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddEntityFrameworkInMemoryDatabase().AddEntityFrameworkSqlite().AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Data Source=PasswordServer.db"));
			services.AddTransient<IStorageService, StorageServiceDb>();
		}
	}
}
