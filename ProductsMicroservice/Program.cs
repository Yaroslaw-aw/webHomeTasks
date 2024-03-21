
using Microsoft.EntityFrameworkCore;
using ProductsMicroservice.Db;
using ProductsMicroservice.Mapping;
using ProductsMicroservice.Repositories.ProductsRepository;

namespace ProductsMicroservice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddMemoryCache();

            builder.Services.AddScoped<IClientRepository, ClientRepository>();

            builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("db")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            AppContext.SetSwitch("Npgsql.EnablelegacyTimestampBehavior", true);

            app.Run();
        }
    }
}
