using Microsoft.EntityFrameworkCore;

namespace Market.Models
{
    public class MarketContext : DbContext
    {
        //public string? connectionString { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Storage> Storages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductStorage> ProductStorages { get; set; }
        public DbSet<CategoryProduct> CategoryProducts { get; set; }

        public MarketContext() { }
        public MarketContext(DbContextOptions<MarketContext> options) : base(options) { Database.EnsureCreated(); }
    }
}
