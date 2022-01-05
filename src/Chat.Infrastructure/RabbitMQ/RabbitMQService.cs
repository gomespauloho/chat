using System;
using Chat.Infrastructure.Configuration;
using Chat.Infrastructure.RabbitMQ.Abstractions;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Chat.Infrastructure.RabbitMQ
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly RabbitMQSettings _rabbitMQSettings;
        private readonly ConnectionFactory _connectionFactory;

        public RabbitMQService(IOptions<RabbitMQSettings> options)
        {
            _rabbitMQSettings = options.Value;

            _connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(_rabbitMQSettings.Host)
            };
        }

        public void Send(byte[] content)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(_rabbitMQSettings.StockQueue, exclusive: false);

            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: _rabbitMQSettings.StockQueue,
                body: content);
        }
    }
}
