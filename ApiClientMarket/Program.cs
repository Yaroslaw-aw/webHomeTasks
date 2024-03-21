
using ApiClientMarket.Db;
using ApiClientMarket.Dto.Mapping;
using ApiClientMarket.Repositories.ClientRepository;
using Microsoft.EntityFrameworkCore;

namespace ApiClientMarket
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddMemoryCache();

            builder.Services.AddScoped<IClientProductRepository, ClientProductRepository>();

            builder.Services.AddDbContext<ClientMarketContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("db")));

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

            app.Run();
        }
    }
}
