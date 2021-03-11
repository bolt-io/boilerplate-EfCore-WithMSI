using Infrastructure.EFCore.MSI;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Infrastructure.EFCore
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DbContext>
    {
        public DbContext CreateDbContext(string[] args)
        {
            var configuration = GetConfiguration();
            var connectionString = configuration["DbContextOptions:ConnectionString"];

            if (string.IsNullOrWhiteSpace(connectionString)) throw new NullReferenceException("Connection String was not found in configuration");

            var tokenProvider = new AzureIdentityAzureSqlTokenProvider(); // no need to worry about caching tokens here.
            var interceptor = new AzureAdAuthenticationDbConnectionInterceptor(tokenProvider);

            // Create a connection to a database used for building scripts
            return new DbContext(new AzureAdDbContextOptions(connectionString, interceptor));
        }

        private IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                                    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../BlazorUI"))
                                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            
            AddEnvironmentJsonFiles(builder);

            builder.AddEnvironmentVariables();

            // Build config
            return builder.Build();
        }

        private static void AddEnvironmentJsonFiles(IConfigurationBuilder builder)
        {
            string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var hasEnvironmentVariable = !string.IsNullOrWhiteSpace(environment);
            if (hasEnvironmentVariable)
                builder.AddJsonFile($"appsettings.{environment}.json", optional: true);
        }
    }
}
