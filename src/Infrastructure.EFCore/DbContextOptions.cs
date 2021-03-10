using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.EFCore
{
    public class DbContextOptions : IDbContext
    {
        public DbContextOptions()
        {
        }

        public DbContextOptions(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; set; }

        public bool UseChangeTracker { get; set; }


        public Microsoft.EntityFrameworkCore.DbContextOptions Options
        {
            get
            {
                var builder = new DbContextOptionsBuilder<DbContext>();
                return builder.UseSqlServer(ConnectionString).Options;
            }
        }
    }
}

