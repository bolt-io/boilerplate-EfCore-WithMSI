using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.EFCore
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DbContext>
    {
        public DbContext CreateDbContext(string[] args)
        {
            // Create a connection to a database used for building scripts
            return new DbContext(new DbContextOptions("Data Source=localhost;Initial Catalog=ThirdSpaceTestHarness;Integrated Security=True"));
        }
    }
}
