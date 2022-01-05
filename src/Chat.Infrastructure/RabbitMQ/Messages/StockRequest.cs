using System;
namespace Chat.Infrastructure.RabbitMQ.Messages
{
    public class StockRequest
    {
        public string Username { get; set; }
        public string StockCode { get; set; }
    }
}
