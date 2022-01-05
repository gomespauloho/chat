using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Domain.Domains;

namespace Chat.Domain.Repositories
{
    public interface IMessageRepository
    {
        Task<Message> SaveAsync(Message message);
        Task<IEnumerable<Message>> GetMessagesAsync();
    }
}
