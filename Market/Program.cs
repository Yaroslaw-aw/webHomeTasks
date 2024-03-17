using Market.DTO.Mapping;
using Market.Models;
using Market.Repositories.CategoryRepo;
using Market.Repositories.ProductRepo;
using Market.Repositories.StorageRepo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;

namespace Market
{
    public class Program
    {
        private static WebApplication AppBuilding(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IStorageRepository, StorageRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                string server = "127.0.0.1";
                string port = "6379";
                string cnstring = $"{server}:{port}";
                options.Configuration = cnstring;
            });

            string? connectionString = builder.Configuration.GetConnectionString("db");
            builder.Services.AddDbContext<MarketContext>(options => options.UseNpgsql(connectionString));

            builder.Services.AddMemoryCache(options => 
            {
                options.TrackStatistics = true;
                options.TrackLinkedCacheEntries = true;
            });

            return builder.Build();
        }

        public static void Main(string[] args)
        {
            var app = AppBuilding(args);  
            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            string staticFilePath = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles");
            Directory.CreateDirectory(staticFilePath);

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(staticFilePath),
                RequestPath = "/static"
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
