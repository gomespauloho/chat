using System.Threading.Tasks;
using Chat.Domain.Domains;
using Chat.Domain.Repositories;
using Chat.Domain.Services;
using Chat.Domain.Services.Abstractions;
using Moq;
using Xunit;

namespace Chat.Unit.Tests.Domain
{
    public class MessageServiceTests
    {
        private readonly IMessageService _messageService;
        private readonly Mock<IMessageRepository> _messageRepository = new Mock<IMessageRepository>();
        private readonly Mock<IBotService> _botService = new Mock<IBotService>();
        private readonly Mock<IChatService> _chatService = new Mock<IChatService>();

        public MessageServiceTests()
        {
            _messageService = new MessageService(
                _chatService.Object,
                _messageRepository.Object,
                _botService.Object);
        }

        [Theory]
        [InlineData("Abc", "Hello", false)]
        [InlineData("Abc", "/stock=aapl.us", true)]
        public async Task ShouldProcessMessageCorrectly(string username, string content, bool isCommand)
        {
            var message = new Message
            {
                Username = username,
                Content = content
            };

            _botService.Setup(x => x.IsValidCommand(It.IsAny<string>())).Returns(isCommand);
            _messageRepository.Setup(x => x.SaveAsync(message)).ReturnsAsync(message);

            await _messageService.CreateMessageAsync(message);

            if (isCommand)
            {
                _messageRepository.Verify(x => x.SaveAsync(message), Times.Never);
                _botService.Verify(x => x.ProcessCommandAsync(message), Times.Once);
                _chatService.Verify(x => x.SendMensageAsync(message), Times.Never);
                return;
            }

            _botService.Verify(x => x.ProcessCommandAsync(message), Times.Never);
            _messageRepository.Verify(x => x.SaveAsync(message), Times.Once);
            _chatService.Verify(x => x.SendMensageAsync(message), Times.Once);
        }
    }
}
