using System.Threading.Tasks;

namespace Chat.Domain.Gateways
{
    public interface IStockGateway
    {
        void SendStockRequestAsync(string username, string stockCode);

        Task<string> GetStockAsync(string stockCode);
    }
}
