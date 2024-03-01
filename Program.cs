
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace TestingEFCoreBehavior
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

            builder.Services.AddScoped<ITenant>(sp =>
            {
                return new Tenant(1); //tenant hardcoded for now
            });

            var shardConfiguration = builder.Configuration.GetSection("ShardConfiguration").Get<ShardConfiguration>();
            builder.Services.AddSingleton(x => new ShardManager(shardConfiguration));

            var shardConnectionString = builder.Configuration.GetConnectionString("TestEFCore");
            builder.Services.AddScoped<InitContext>();
            builder.Services.AddDbContextPool<TestContext>(options => options.UseSqlServer(shardConnectionString));

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
