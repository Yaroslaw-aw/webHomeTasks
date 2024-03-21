using Autofac;
using Market.DTO.Mapping;
using Market.Models.Context;
using Market.Repositories.CategoryRepo;
using Market.Repositories.ProductRepo;
using Market.Repositories.StorageRepo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace Market
{
    public class Program
    {
        private static WebApplication AppBuilding(string[] args)
        {
            WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            ConfigurationBuilder? config = new ConfigurationBuilder();
            config.AddJsonFile("appsettings.json");
            IConfigurationRoot? cfg = config.Build();

            //builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
            //{
            //    containerBuilder.Register(c => new MarketContext(cfg.GetConnectionString("db"))).InstancePerDependency();
            //});

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

            builder.Services.AddMemoryCache(options =>
            {
                options.TrackStatistics = true;
                options.TrackLinkedCacheEntries = true;
            });

            string? connectionString = builder.Configuration.GetConnectionString("db");
            builder.Services.AddDbContext<MarketContext>(options => options.UseNpgsql(connectionString));

            

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
