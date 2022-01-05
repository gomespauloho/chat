using Chat.Domain.Hubs;
using Chat.Domain.Services;
using Chat.Domain.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Domain.Configuration
{
    public static class ConfigurationExtensions
    {
        public static void ConfigureDomain(this IServiceCollection services)
        {
            services.AddSignalR();

            services.AddSingleton<IChatService, ChatService>();
            services.AddSingleton<IBotService, BotService>();
            services.AddSingleton<IMessageService, MessageService>();
            
        }
    }
}
