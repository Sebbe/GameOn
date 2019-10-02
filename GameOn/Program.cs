using GameOn.Web.Infrastructure.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GameOn.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .ConfigureAppConfiguration(AddDbConfiguration)
                   .UseStartup<Startup>()
                   .Build();

        private static void AddDbConfiguration(WebHostBuilderContext context, IConfigurationBuilder builder)
        {
            var configuration    = builder.Build();
            var connectionString = configuration.GetConnectionString("Database");
            builder.AddAdminConfigurationBuilder(options =>
            {
                options.UseSqlServer(connectionString);
            });
        }
    }
}
