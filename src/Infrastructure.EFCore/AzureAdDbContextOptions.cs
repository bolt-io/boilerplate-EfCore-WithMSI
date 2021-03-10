using Infrastructure.EFCore.MSI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.EFCore
{
    public class AzureAdDbContextOptions : IDbContext
    {
        public AzureAdDbContextOptions(string connectionString, AzureAdAuthenticationDbConnectionInterceptor dbConnectionInterceptor)
        {
            ConnectionString = connectionString;
            _dbConnectionInterceptor = dbConnectionInterceptor;
        }

        public string ConnectionString { get; set; }
        private AzureAdAuthenticationDbConnectionInterceptor _dbConnectionInterceptor;
        public bool UseChangeTracker { get; set; }


        public Microsoft.EntityFrameworkCore.DbContextOptions Options
        {
            get
            {
                var builder = new DbContextOptionsBuilder<DbContext>();
                return builder.UseSqlServer(ConnectionString, options => options.EnableRetryOnFailure())
                    .AddInterceptors(_dbConnectionInterceptor).Options;
            }
        }
    }
}

