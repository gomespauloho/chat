using System.Threading.Tasks;
using Chat.Domain.Domains;
using Chat.Domain.Hubs;
using Chat.Domain.Services.Abstractions;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Domain.Services
{
    public class ChatService : IChatService
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatService(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendMensageAsync(Message message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
