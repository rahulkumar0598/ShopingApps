using Npgsql;

namespace Discount.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host,int? retry=0)
        {
            int retryForAvailability = retry.Value;
            using(var scope=host.Services.CreateScope())
            {
                var services=scope.ServiceProvider;
                var configuration=services.GetRequiredService<IConfiguration>();
                var logger=services.GetRequiredService<ILogger<TContext>>();
                try
                {
                    logger.LogInformation("Migrate postgresql database");
                    using var connection = new NpgsqlConnection
                        (configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                    connection.Open();
                    using var command = new NpgsqlCommand
                    {
                        Connection = connection,
                    };
                    command.CommandText = "DROP TABLE IF EXISTS Coupon";
                    //command.ExecuteNonQuery() helps to run query in database
                    command.ExecuteNonQuery();

                    command.CommandText = @"CREATE TABLE Coupon (Id SERIAL PRIMARY KEY,
                                           ProductName VARCHAR(24) NOT NULL,
                                            Description TEXT,
                                            Amount INT)";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO Coupon(ProductName,Description,Amount) VALUES ('IPhone X','IPhone X Discount',150);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT INTO Coupon(ProductName,Description,Amount) VALUES ('Samsung ','Samsung  Discount',250);";
                    command.ExecuteNonQuery();

                    logger.LogInformation("Migrate postgresql database");


                }
                catch (NpgsqlException ex)
                {
                    logger.LogError(ex, "An error occured while migrating postgesql database");
                    if(retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        System.Threading.Thread.Sleep(2000);//waiting 2 seconds
                        MigrateDatabase<TContext>(host, retryForAvailability);
                    }

                }
            }
            return host;

        }
    }
}
