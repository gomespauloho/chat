using System.Threading.Tasks;
using Chat.Domain.Gateways;
using Chat.Domain.Services.Abstractions;
using Chat.Infrastructure.Configuration;
using Chat.Infrastructure.Mappers;
using Chat.Infrastructure.RabbitMQ.Messages;
using Microsoft.Extensions.Options;

namespace Chat.Infrastructure.RabbitMQ
{
    public class ProcessMessageConsumer : BaseConsumer
    {
        private readonly IStockGateway _stockGateway;
        private readonly IBotService _botService;

        public ProcessMessageConsumer(IOptions<RabbitMQSettings> options,
            IStockGateway stockGateway,
            IBotService botService) : base(options)
        {
            _stockGateway = stockGateway;
            _botService = botService;
        }

        protected override async Task ProcessMessageAsync(StockRequest message)
        {
            var result = await _stockGateway.GetStockAsync(message.StockCode);

            var stock = message.ParseCsvToStock(result);

            await _botService.ProcessCallbackStockAsync(stock);
        }
    }
}
