using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Chat.Domain.Gateways;
using Chat.Infrastructure.Configuration;
using Chat.Infrastructure.RabbitMQ.Abstractions;
using Chat.Infrastructure.RabbitMQ.Messages;
using Microsoft.Extensions.Options;
using Polly;
using RabbitMQ.Client;

namespace Chat.Infrastructure.Gateways
{
    public class StockGateway : IStockGateway
    {
        private readonly IRabbitMQService _rabbitMQService;
        private readonly IStockApi _stockApi;

        public StockGateway(IRabbitMQService rabbitMQService, IStockApi stockApi)
        {
            _rabbitMQService = rabbitMQService;
            _stockApi = stockApi;
        }

        public void SendStockRequestAsync(string username, string stockCode)
        {
            var body = GetBody(username, stockCode);

            _rabbitMQService.Send(body);
        }

        private static byte[] GetBody(string username, string stockCode)
        {
            var stockRequest = new StockRequest
            {
                Username = username,
                StockCode = stockCode
            };

            var json = JsonSerializer.Serialize(stockRequest);

            return Encoding.UTF8.GetBytes(json);
        }

        public async Task<string> GetStockAsync(string stockCode)
        {
            return await Policy<string>
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .ExecuteAsync(() => _stockApi.GetStockAsync(stockCode));
        }
    }
}
