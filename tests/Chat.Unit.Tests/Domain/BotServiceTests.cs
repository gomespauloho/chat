using System.Threading.Tasks;
using Chat.Domain.Domains;
using Chat.Domain.Gateways;
using Chat.Domain.Services;
using Chat.Domain.Services.Abstractions;
using FluentAssertions;
using Moq;
using Xunit;

namespace Chat.Unit.Tests.Domain
{
    public class BotServiceTests
    {
        private readonly IBotService _botService;
        private readonly Mock<IChatService> _chatService = new Mock<IChatService>();
        private readonly Mock<IStockGateway> _stockGateway = new Mock<IStockGateway>();

        public BotServiceTests()
        {
            _botService = new BotService(_chatService.Object, _stockGateway.Object);
        }

        [Theory]
        [InlineData("Hi", false)]
        [InlineData("/stock", false)]
        [InlineData("/stock=", false)]
        [InlineData("stock=", false)]
        [InlineData("stock=aapl.us", false)]
        [InlineData("/stock=aapl.us", true)]
        [InlineData("Hi /stock=aapl.us", false)]
        public void ValidateCommand(string command, bool expected)
        {
            _botService.IsValidCommand(command).Should().Be(expected);
        }

        [Theory]
        [InlineData("Abc", "stock", "aapl.us", true)]
        [InlineData("Abc", "getstock", "aapl.us", false)]
        [InlineData("Abc", "stocks", "aapl.us", false)]
        public async Task ShouldProcessCommandCorrectly(
            string username,
            string command,
            string stockCode,
            bool isValidCommand)
        {
            var message = new Message
            {
                Username = username,
                Content = $"/{command}={stockCode}"
            };


            Message sentMessage = default;

            _chatService
                .Setup(x => x.SendMensageAsync(It.IsAny<Message>()))
                .Callback<Message>(message => sentMessage = message);

            await _botService.ProcessCommandAsync(message);

            if (isValidCommand)
            {
                _stockGateway.Verify(x => x.SendStockRequestAsync(username, stockCode), Times.Once);
                sentMessage.Should().BeNull();
                return;
            }

            _stockGateway.Verify(x => x.SendStockRequestAsync(username, stockCode), Times.Never);
            sentMessage.Username.Should().Be("Bot");
            sentMessage.Content.Should().Be($"Unrecognized '{command}' command, try /stock=stock_code");
        }

        [Theory]
        [InlineData("AAPL.US", "123", true)]
        [InlineData("APL.US", "N/D", false)]
        [InlineData("ABC", "N/D", false)]
        public async Task ShouldProcessCallbackStockCorrectly(
            string code,
            string amount,
            bool isvalid)
        {
            var stock = new Stock
            {
                Code = code,
                Amount = amount
            };

            var expectedMessage = isvalid ?
                $"{stock.Code} quote is ${stock.Amount} per share" :
                $"Stock code {stock.Code} is invalid";


            Message sentMessage = default;

            _chatService
                .Setup(x => x.SendMensageAsync(It.IsAny<Message>()))
                .Callback<Message>(message => sentMessage = message);

            await _botService.ProcessCallbackStockAsync(stock);

            sentMessage.Should().NotBeNull();
            sentMessage.Username.Should().Be("Bot");
            sentMessage.Content.Should().Be(expectedMessage);
        }
    }
}
