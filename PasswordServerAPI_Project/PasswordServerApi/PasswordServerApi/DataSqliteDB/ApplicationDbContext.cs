using Microsoft.EntityFrameworkCore;

namespace PasswordServerApi.DataSqliteDB
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}

		public DbSet<EndityAbstractModelAccount> Accounts { get; set; }

		public DbSet<EndityAbstractModelPassword> Passwords { get; set; }

    }
}
