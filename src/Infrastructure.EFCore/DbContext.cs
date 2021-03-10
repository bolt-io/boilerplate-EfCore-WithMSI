using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EFCore
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        private const string DefaultDbSchema = "Config";

        internal bool UseChangeTracker { get; }

        public DbSet<SampleEntity> SampleEntities { get; set; }

        private DbContext([NotNull] Microsoft.EntityFrameworkCore.DbContextOptions options) : base(options)
        {
        }
        
        public DbContext([NotNull] IDbContext options) : this(options.Options)
        {
            // Database migration is managed through pipeline. You can manually push database changes via PM> update-database
            // DO NOT USE Database.Migrate();

            UseChangeTracker = options.UseChangeTracker;
            ConfigureChangeTracker();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema(DefaultDbSchema);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ConfigureChangeTracker()
        {
            base.ChangeTracker.AutoDetectChangesEnabled = UseChangeTracker; // disable auto detect changes if using stored procedures instead of calling .SaveChanges()
        }
    }
}