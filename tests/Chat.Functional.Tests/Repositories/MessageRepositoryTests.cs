using System;
using System.Linq;
using System.Threading.Tasks;
using Chat.Domain.Domains;
using Chat.Domain.Repositories;
using Chat.Infrastructure.Repositories;
using FluentAssertions;
using Mongo2Go;
using MongoDB.Driver;
using Xunit;

namespace Chat.Functional.Tests.Repositories
{
    public class MessageRepositoryTests : IDisposable
    {
        private readonly MongoDbRunner _runner;
        private readonly IMessageRepository _messageRepository;

        public MessageRepositoryTests()
        {
            _runner = MongoDbRunner.Start();

            var client = new MongoClient(_runner.ConnectionString);
            var database = client.GetDatabase("IntegrationTest");

            _messageRepository = new MessageRepository(database);
        }

        [Fact]
        public async Task ShouldSaveMessage()
        {
            var message = new Message
            {
                Username = "User1",
                Content = "Hello!"
            };

            var result = await _messageRepository.SaveAsync(message);

            result.Should().NotBeNull();
            result.CreatedAt.Should().NotBeNull();
            result.Username.Should().Be(message.Username);
            result.Content.Should().Be(message.Content);
        }

        [Fact]
        public async Task ShouldGetMessages()
        {
            for(var i = 1; i <= 100; i++)
            {
                var message = new Message
                {
                    Username = $"User{i}",
                    Content = "Hello!"
                };

                await _messageRepository.SaveAsync(message);
            }


            var result = await _messageRepository.GetMessagesAsync();

            result.Count().Should().Be(50);
            result.First().Username.Should().Be("User100");
            result.Last().Username.Should().Be("User51");
        }

        public void Dispose()
        {
            _runner?.Dispose();
        }
    }
}
