using Microsoft.EntityFrameworkCore;

namespace Market.Models.Context
{
    public class MarketContext : DbContext
    {
        //private string? connectionString;
        public DbSet<Product> Products { get; set; }
        public DbSet<Storage> Storages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductStorage> ProductStorages { get; set; }
        public DbSet<CategoryProduct> CategoryProducts { get; set; }

        
        public MarketContext(DbContextOptions<MarketContext> options) : base(options) { Database.EnsureCreated(); }
    }
}
