using Microsoft.EntityFrameworkCore;

namespace Market.Models
{
    public class MarketContext : DbContext
    {
        public string? connectionString { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Storage> Storages { get; set; }
        public DbSet<Category> Categories { get; set; }

        public MarketContext() { }
        public MarketContext(DbContextOptions<MarketContext> options) : base(options) { Database.EnsureCreated(); }
        //public MarketContext(string? connectionString)
        //{
        //    this.connectionString = connectionString;
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)        
        //    => optionsBuilder.UseNpgsql(connectionString);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products");

                entity.HasKey(x => x.Id).HasName("product_pkey");
                entity.HasIndex(x => x.Name).IsUnique();

                entity.Property(e => e.Id)
                      .HasColumnName("ProductID");

                entity.Property(e => e.Name)
                      .HasColumnName("Product Name")
                      .HasMaxLength(255);

                entity.Property(e => e.Description)
                      .HasColumnName("Description")
                      .HasMaxLength(255);

                entity.Property(e => e.Price)
                      .HasColumnName("Price")
                      .IsRequired();

                entity.HasOne(x => x.Category)
                      .WithMany(c => c.Products)
                      .HasForeignKey(x => x.Id)
                      .HasConstraintName("product_to_groups");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("ProductCategories");

                entity.HasKey(x => x.Id).HasName("Group_pkey");
                entity.HasIndex(x => x.Name).IsUnique();

                entity.Property(e => e.Id)
                      .HasColumnName("CategoryID");

                entity.Property(e => e.Name)
                      .HasColumnName("Category Name")
                      .HasMaxLength(255);

                entity.Property(e => e.Description)
                      .HasColumnName("Categoty description")
                      .HasMaxLength(255);
            });

            modelBuilder.Entity<Storage>(entity =>
            {
                entity.ToTable("Storage");

                entity.HasKey(x => x.Id).HasName("StoreID");

                entity.Property(e => e.Id)
                      .HasColumnName("StorageID");

                entity.Property(e => e.Name)
                      .HasColumnName("StorageName")
                      .HasMaxLength(255);

                entity.Property(e => e.Description)
                      .HasColumnName("StorageDescription")
                      .HasMaxLength(255);

                entity.HasMany(x => x.Products)
                      .WithMany(m => m.Storages)
                      .UsingEntity(j => j.ToTable("StorageProduct"));
            });
        }
    }
}
