using System;
namespace Chat.Infrastructure.RabbitMQ.Abstractions
{
    public interface IRabbitMQService
    {
        void Send(byte[] content);
    }
}
