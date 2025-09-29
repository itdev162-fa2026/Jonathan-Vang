using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        // EF will set this at runtime; null! silences the CS8618 warning.
        public DbSet<Product> Products { get; set; } = null!;
    }
}
