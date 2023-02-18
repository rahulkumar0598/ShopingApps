using Discount.gRPC.Extensions;
using Discount.gRPC.Repository;
using Discount.gRPC.Services;
using Microsoft.Extensions.Logging;
//using Discount.gRPC.Services;

internal class Program
{
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args);
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        var host = CreateHostBuilder(args).Build();
        host.MigrateDatabase<Program>();

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

        // Add services to the container.
        builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
        builder.Services.AddAutoMapper(typeof(Program));
        builder.Services.AddGrpc();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.MapGrpcService<DiscountService>();
        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }
}