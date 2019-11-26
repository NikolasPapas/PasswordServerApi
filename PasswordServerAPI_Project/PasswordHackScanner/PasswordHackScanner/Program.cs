using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace PasswordHackScanner
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
               // webBuilder.UseUrls("http://*:51360;https://hostname:51360")
               webBuilder.UseUrls("http://*:51360");
               webBuilder.UseStartup<Startup>();
           });
    }
}
