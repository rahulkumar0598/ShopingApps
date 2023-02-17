using Discount.API.Extensions;
using Discount.API.Repository;

internal class Program
{
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args);
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var host = CreateHostBuilder(args).Build();
        host.MigrateDatabase<Program>();



        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}