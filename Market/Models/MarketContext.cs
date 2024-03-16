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

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<CategoryProduct>()
        //        .HasKey(cp => new { cp.CategoryId, cp.ProductId });

        //    modelBuilder.Entity<CategoryProduct>()
        //        .HasOne(cp => cp.Product)
        //        .WithMany(p => p.CategoryProducts)
        //        .HasForeignKey(cp => cp.ProductId)
        //        .OnDelete(DeleteBehavior.Cascade); // Установите каскадное удаление, если нужно

        //    modelBuilder.Entity<CategoryProduct>()
        //        .HasOne(cp => cp.Category)
        //        .WithMany()
        //        .HasForeignKey(cp => cp.CategoryId);

        //    modelBuilder.Entity<Product>()
        //        .HasOne(p => p.Category)
        //        .WithMany(c => c.Products)
        //        .HasForeignKey(p => p.CategoryId);
        //}

    }
}
