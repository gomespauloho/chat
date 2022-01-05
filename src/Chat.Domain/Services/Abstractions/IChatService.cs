using System.Threading.Tasks;
using Chat.Domain.Domains;

namespace Chat.Domain.Services.Abstractions
{
    public interface IChatService
    {
        Task SendMensageAsync(Message message);
    }
}
