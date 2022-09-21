using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Requests.Abstractions;
using System;


namespace MaerskScheduleBot
{
    internal class TelegramHandler
    {
        public TelegramBotClient client;
        public VesselsManager vesselsManager;
        public TelegramHandler(TelegramBotClient client, VesselsManager vesselsManager)
        {
            this.client = client;
            this.vesselsManager = vesselsManager;
        }
        public async Task TelegramUpdate(ITelegramBotClient client, Update update, CancellationToken cancelToken)
        {
            if (update.Message != null)
            {
                var message = update.Message;
                var chat = client.GetChatAsync(message.Chat.Id).Result;

                Console.WriteLine(message.From.FirstName + " " + message.From.LastName + ":");
                Console.WriteLine(message.Text);

                MessageHandler messageHandler = new(this, vesselsManager, message, chat);
                await messageHandler.ProceedMessage();
            }
            return;
        }
        public Task TelegramError(ITelegramBotClient client, Exception exception, CancellationToken cancelToken)
        {
            Console.WriteLine("Error");
            throw exception;
        }
    }
}