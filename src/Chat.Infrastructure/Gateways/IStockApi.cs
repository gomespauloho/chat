using System.Threading.Tasks;
using Refit;

namespace Chat.Infrastructure.Gateways
{
    public interface IStockApi
    {
        [Get("/q/l/?s={stockCode}&f=sd2t2ohlcv&h&e=csv")]
        Task<string> GetStockAsync(string stockCode);
    }
}
