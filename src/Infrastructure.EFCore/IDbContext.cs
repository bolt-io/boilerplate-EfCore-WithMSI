
namespace Infrastructure.EFCore
{
    public interface IDbContext
    {
        string ConnectionString { get; set; }

        bool UseChangeTracker { get; set; }

        Microsoft.EntityFrameworkCore.DbContextOptions Options { get; }
    }
}