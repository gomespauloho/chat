using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Domain.Domains;
using Chat.Domain.Repositories;
using Chat.Domain.Services.Abstractions;

namespace Chat.Domain.Services
{
    public class MessageService : IMessageService
    {
        private readonly IChatService _chatService;
        private readonly IMessageRepository _messageRepository;
        private readonly IBotService _botService;

        public MessageService(
            IChatService chatService,
            IMessageRepository messageRepository,
            IBotService botService)
        {
            _chatService = chatService;
            _messageRepository = messageRepository;
            _botService = botService;
        }

        public async Task CreateMessageAsync(Message message)
        {
            if (_botService.IsValidCommand(message.Content))
            {
                await _botService.ProcessCommandAsync(message);
                return;
            }

            var result = await _messageRepository.SaveAsync(message);

            await _chatService.SendMensageAsync(result);
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync()
        {
            var list = await _messageRepository.GetMessagesAsync();

            return list.OrderBy(x => x.CreatedAt);
        }
    }
}
