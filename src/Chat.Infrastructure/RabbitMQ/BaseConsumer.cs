using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Chat.Infrastructure.Configuration;
using Chat.Infrastructure.RabbitMQ.Messages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Chat.Infrastructure.RabbitMQ
{
    public abstract class BaseConsumer : IDisposable, IHostedService
    {
        private readonly RabbitMQSettings _rabbitMQSettings;
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;

        public BaseConsumer(IOptions<RabbitMQSettings> options)
        {
            _rabbitMQSettings = options.Value;

            _connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(_rabbitMQSettings.Host),
                DispatchConsumersAsync = true
            };
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(_rabbitMQSettings.StockQueue, exclusive: false);

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.Received += new AsyncEventHandler<BasicDeliverEventArgs>(EventHandlerAsync);

            _channel.BasicConsume(_rabbitMQSettings.StockQueue, true, consumer);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _channel?.Close();
            _connection?.Close();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }

        private async Task EventHandlerAsync(object sender, BasicDeliverEventArgs eventArgs)
        {
            var body = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
            var message = JsonSerializer.Deserialize<StockRequest>(body);

            await ProcessMessageAsync(message);
        }

        protected abstract Task ProcessMessageAsync(StockRequest message);
    }
}
