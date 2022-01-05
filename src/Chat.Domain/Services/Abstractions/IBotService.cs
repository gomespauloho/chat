using System.Threading.Tasks;
using Chat.Domain.Domains;

namespace Chat.Domain.Services.Abstractions
{
    public interface IBotService
    {
        Task ProcessCommandAsync(Message message);
        bool IsValidCommand(string content);
        Task ProcessCallbackStockAsync(Stock stock);
    }
}
