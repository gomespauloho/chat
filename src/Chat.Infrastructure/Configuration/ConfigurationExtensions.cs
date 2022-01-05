using Refit;
using Chat.Domain.Repositories;
using Chat.Infrastructure.Gateways;
using Chat.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Chat.Infrastructure.RabbitMQ;
using Chat.Domain.Gateways;
using MongoDB.Driver;
using Chat.Infrastructure.RabbitMQ.Abstractions;

namespace Chat.Infrastructure.Configuration
{
    public static class ConfigurationExtensions
    {
        public static void ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseSettings>(configuration.GetSection(nameof(DatabaseSettings)));
            services.Configure<RabbitMQSettings>(configuration.GetSection(nameof(RabbitMQSettings)));

            AddRefit(services, configuration);
            AddMongoDB(services, configuration);

            services.AddSingleton<IMessageRepository, MessageRepository>();
            services.AddSingleton<IStockGateway, StockGateway>();
            services.AddSingleton<IRabbitMQService, RabbitMQService>();

            services.AddHostedService<ProcessMessageConsumer>();
        }

        private static void AddRefit(this IServiceCollection services, IConfiguration configuration)
        {
            var stockSettings = new StockSettings();

            configuration.Bind(nameof(StockSettings), stockSettings);

            services.AddRefitClient<IStockApi>()
                .ConfigureHttpClient(x => x.BaseAddress = new Uri(stockSettings.UrlApi));
        }

        private static void AddMongoDB(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseSettings = new DatabaseSettings();

            configuration.Bind(nameof(DatabaseSettings), databaseSettings);

            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            services.AddSingleton(database);
        }
    }
}
