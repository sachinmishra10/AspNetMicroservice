﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Discount.API.Extensions
{
    public static class HostExtension
    {
        public static IHost MigrateDatabase<T>(this IHost host, int retry = 0)
        {
            var retryForAvailability = retry;
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configurationManager = services.GetService<IConfiguration>();
                var logger = services.GetService<ILogger>();
                try
                {
                    using (var connection = new NpgsqlConnection(configurationManager.GetValue<string>("DatabaseSettings:ConnectionString")))
                    {
                        connection.Open();

                        using var command = new NpgsqlCommand
                        {
                            Connection = connection
                        };

                        command.CommandText = "DROP TABLE IF EXISTS Coupon";
                        command.ExecuteNonQuery();

                        command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, 
                                                                ProductName VARCHAR(24) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT)";
                        command.ExecuteNonQuery();

                        command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('IPhone X', 'IPhone Discount', 150);";
                        command.ExecuteNonQuery();

                        command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Samsung 10', 'Samsung Discount', 100);";
                        command.ExecuteNonQuery();

                        logger.LogInformation("Migrated postresql database.");
                    }
                }
                catch (NpgsqlException ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the postresql database");

                    if (retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        System.Threading.Thread.Sleep(2000);
                        MigrateDatabase<T>(host, retryForAvailability);
                    }
                }
                return host;
            }
        }
    }
}
