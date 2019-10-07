using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace PasswordServerApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateWebHostBuilder(args).Build().Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
			.UseUrls("http://*:53257;https://hostname:44390")
				.UseStartup<Startup>()
				.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
					.ReadFrom.Configuration(hostingContext.Configuration));

	}
}
