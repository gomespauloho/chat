using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Domain.Domains;

namespace Chat.Domain.Services.Abstractions
{
    public interface IMessageService
    {
        Task CreateMessageAsync(Message message);
        Task<IEnumerable<Message>> GetMessagesAsync();
    }
}
