using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Cookbook.Repository.DbContexts;

namespace Cookbook.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var x = new CookbookContext();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
