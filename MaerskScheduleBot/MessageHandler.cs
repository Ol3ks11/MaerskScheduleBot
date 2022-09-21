using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Requests.Abstractions;

namespace MaerskScheduleBot
{
    internal class MessageHandler
    {

        readonly TelegramHandler telegramHandler;
        readonly VesselsManager vesselsManager;
        readonly Message message;
        readonly Chat chat;

        public MessageHandler(TelegramHandler telegramHandler, VesselsManager vesselsManager, Message message, Chat chat)
        {
            this.telegramHandler = telegramHandler;
            this.vesselsManager = vesselsManager;
            this.message = message;
            this.chat = chat;
        }

        public async Task ProceedMessage()
        {
            if (message.Text != null && message.Text[0] == '/')
            {
                await ExecuteCommand();
            }
            else if (message.Text != null && chat.PinnedMessage == null)
            {
                await CheckIfNameLegit();
            }
        }

        private async Task CheckIfNameLegit()
        {
            List<Ship> shipList = vesselsManager.ships.FindAll(ship => ship.ShipName.Contains(message.Text.ToUpper()));

            if (shipList.Count != 0)
            {
                if (shipList.Count < 2)
                {
                    var messageToPin = await telegramHandler.client.SendTextMessageAsync(chat.Id, shipList[0].ShipName);
                    await telegramHandler.client.PinChatMessageAsync(chat, messageToPin.MessageId);
                    Console.WriteLine("Match found.");
                    return;
                }
                await telegramHandler.client.SendTextMessageAsync(chat.Id, "Found more then one match, be more specific.");
                Console.WriteLine("Found more then one match.");
                return;
            }
            await telegramHandler.client.SendTextMessageAsync(chat.Id, "Can not find a vessel with a matching name.");
            Console.WriteLine("Can not find a vessel with a matching name.");
            return;
        }

        private async Task ExecuteCommand()
        {
            switch (message.Text)
            {
                case "/setup":
                    await Setup();
                    break;

                case "/refresh":
                    await Refresh();
                    break;
            }
        }

        #region Commands
        private async Task Setup()
        {
            await telegramHandler.client.UnpinAllChatMessages(chat.Id);
            await telegramHandler.client.SendTextMessageAsync(chat.Id, "Please enter Vessel name.");
            Console.WriteLine("Setup command");
        }
        private async Task Refresh()
        {
            if (chat.PinnedMessage == null)
            {
                await telegramHandler.client.SendTextMessageAsync(message.Chat.Id, "Please setup your Vessel via /Setup command.");
            }
            else if (chat.PinnedMessage != null)
            {
                await SendSchedule();
            }
            Console.WriteLine("Refresh command");
        }
        private async Task SendSchedule()
        {
            var ship = vesselsManager.ships.Find(ship => ship.ShipName == chat.PinnedMessage.Text);
            int shipIndex = vesselsManager.ships.IndexOf(ship);
            vesselsManager.UpdateShipPorts(ship.ShipName);
            await telegramHandler.client.SendTextMessageAsync(chat.Id, vesselsManager.BuildSchedule(shipIndex));
        }
        #endregion

    }
}