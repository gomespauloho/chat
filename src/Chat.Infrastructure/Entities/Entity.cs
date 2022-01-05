using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Chat.Infrastructure.Entities
{
    public abstract class Entity
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        public ObjectId Id { get; set; }
        public DateTime CreatedAt => Id.CreationTime.ToLocalTime();
    }
}
