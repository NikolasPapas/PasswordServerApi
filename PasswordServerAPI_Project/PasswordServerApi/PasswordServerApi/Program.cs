using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace PasswordServerApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
           .ConfigureWebHostDefaults(webBuilder =>
           {
               //webBuilder.UseUrls("http://*:53257;https://hostname:44390");
               webBuilder.UseUrls("http://*:53257");
               webBuilder.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));
               webBuilder.UseStartup<Startup>();
           });



        //     public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //WebHost.CreateDefaultBuilder(args)
        ////.UseUrls("http://*:53257;https://hostname:44390")
        //.UseUrls("http://*:53257")
        //	.UseStartup<Startup>()
        //	.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
        //		.ReadFrom.Configuration(hostingContext.Configuration));

    }
}
