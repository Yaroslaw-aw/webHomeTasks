using Microsoft.EntityFrameworkCore;

namespace ProductsMicroservice.Db
{
    // "Host=localhost;Username=postgres;Password=example;Database=MarketProducts"
    public partial class AppDbContext : DbContext
    {
        public string? connectionString;

        public DbSet<Client> Clients { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("clients");

                entity.HasKey(client => client.Id).HasName("clients_pkey");
                entity.HasIndex(product => product.Id).IsUnique();

                entity.Property(product => product.Id)
                .HasColumnName("clientId")
                .IsRequired();

                entity.Property(product => product.Name)
                .HasColumnName("name")
                .IsRequired();

                entity.Property(product => product.Email)
                .HasColumnName("email")
                .IsRequired();
            });
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
