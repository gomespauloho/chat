using System.Text;
using System.Text.Json;
using Chat.Domain.Gateways;
using Chat.Infrastructure.Gateways;
using Chat.Infrastructure.RabbitMQ.Abstractions;
using Chat.Infrastructure.RabbitMQ.Messages;
using Moq;
using Xunit;

namespace Chat.Unit.Tests.Infrastructure
{
    public class StockGatewayTests
    {
        [Fact]
        public void ShouldSendToRabbitMQCorrectly()
        {
            var stockRequest = new StockRequest
            {
                Username = "User1",
                StockCode = "aapl.us"
            };

            var json = JsonSerializer.Serialize(stockRequest);

            var expected =  Encoding.UTF8.GetBytes(json);

            var rabbitMQService = new Mock<IRabbitMQService>();

            IStockGateway gateway = new StockGateway(rabbitMQService.Object,
                new Mock<IStockApi>().Object);

            gateway.SendStockRequestAsync(stockRequest.Username, stockRequest.StockCode);

            rabbitMQService.Verify(x => x.Send(expected), Times.Once);
        }
    }
}
