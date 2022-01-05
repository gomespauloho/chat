using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Chat.Domain.Domains;
using Chat.Domain.Gateways;
using Chat.Domain.Services.Abstractions;

namespace Chat.Domain.Services
{
    public class BotService : IBotService
    {
        private static string _regex = @"^\/([a-z])+=.*\w";
        private static string _stockCommand = "stock";

        private readonly IChatService _chatService;
        private readonly IStockGateway _stockGateway;

        public BotService(IChatService chatService, IStockGateway stockGateway)
        {
            _chatService = chatService;
            _stockGateway = stockGateway;
        }

        public async Task ProcessCommandAsync(Message message)
        {
            var (command, param) = ExtractParam(message.Content);

            if (!IsStockCommand(command))
            {
                await SendCallbackMessage($"Unrecognized '{command}' command, try /stock=stock_code");
                return;
            }

            _stockGateway.SendStockRequestAsync(message.Username, param);
        }

        public bool IsValidCommand(string content) => Regex.IsMatch(content, _regex);

        public async Task ProcessCallbackStockAsync(Stock stock)
        {
            if (IsInvalidStock(stock))
            {
                await SendCallbackMessage($"Stock code {stock.Code} is invalid");
                return;
            }

            await SendCallbackMessage($"{stock.Code} quote is ${stock.Amount} per share");
        }

        private static bool IsInvalidStock(Stock stock) => stock.Amount == "N/D";

        private static (string command, string param) ExtractParam(string content)
        {
            var equalsIdx = content.IndexOf("=");
            var command = content.Substring(1, equalsIdx - 1);
            var param = content.Substring(equalsIdx + 1);

            return (command, param);
        }

        private static bool IsStockCommand(string command) => _stockCommand == command;

        private async Task SendCallbackMessage(string content)
        {
            var botMessage = new Message
            {
                Username = "Bot",
                Content = content,
                CreatedAt = DateTime.Now
            };

            await _chatService.SendMensageAsync(botMessage);
        }
    }
}
