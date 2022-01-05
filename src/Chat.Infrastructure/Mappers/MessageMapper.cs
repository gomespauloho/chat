using Chat.Domain.Domains;
using Chat.Infrastructure.Entities;
using MongoDB.Bson;

namespace Chat.Infrastructure.Mappers
{
    public static class MessageMapper
    {
        public static MessageEntity ToEntity(this Message message) =>
            new MessageEntity
            {
                Id = ObjectId.GenerateNewId(),
                Username = message.Username,
                Content = message.Content
            };

        public static Message ToDomain(this MessageEntity entity) =>
            new Message
            {
                Username = entity.Username,
                Content = entity.Content,
                CreatedAt = entity.CreatedAt
            };
    }
}
