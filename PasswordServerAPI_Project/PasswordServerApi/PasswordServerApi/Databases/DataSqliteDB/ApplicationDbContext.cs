using Microsoft.EntityFrameworkCore;
using PasswordServerApi.Databases.DataModels;

namespace PasswordServerApi.DataSqliteDB
{
	public class ApplicationDbContext : DbContext
	{
		private static bool _created = false;
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
			if (!_created)
			{
				_created = true;
				Database.EnsureDeleted();
				Database.EnsureCreated();
			}
		}
		
		public DbSet<EndityAbstractModelAccount> Accounts { get; set; }

		public DbSet<EndityAbstractModelPassword> Passwords { get; set; }

		public DbSet<LoginTokenModel> LoginTokens { get; set; }

	}
}
