
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace nApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("ocelot.json").Build();

            builder.Services.AddOcelot(configuration);
            builder.Services.AddSwaggerForOcelot(configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerForOcelotUI(opts =>
                {
                    opts.PathToSwaggerGenerator = "/swagger/docs";
                }).UseOcelot().Wait();
                //app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //app.UseAuthorization();            

            app.Run();
        }
    }
}
