using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Domain.Domains;
using Chat.Domain.Repositories;
using Chat.Infrastructure.Entities;
using Chat.Infrastructure.Mappers;
using MongoDB.Driver;

namespace Chat.Infrastructure.Repositories
{
    public class MessageRepository : BaseRepository<MessageEntity>, IMessageRepository
    {
        public MessageRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync()
        {
            var options = new FindOptions<MessageEntity>
            {
                Limit = 50,
                Sort = Builders<MessageEntity>.Sort.Descending(x => x.Id)
            };

            using var cursor = await Collection.FindAsync(_ => true, options);

            var list = await cursor.ToListAsync();

            return list.Select(x => x.ToDomain());
        }

        public async Task<Message> SaveAsync(Message message)
        {
            var entity = message.ToEntity();
            var update = Builders<MessageEntity>.Update
                .Set(x => x.Username, entity.Username)
                .Set(x => x.Content, entity.Content);

            var options = new FindOneAndUpdateOptions<MessageEntity>
            {
                 IsUpsert = true,
                 ReturnDocument = ReturnDocument.After
            };

            var result = await Collection.FindOneAndUpdateAsync<MessageEntity>(x => x.Id == entity.Id, update, options);

            return result.ToDomain();
         }
    }
}
 