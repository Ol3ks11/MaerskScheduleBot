using Telegram.Bot;
using Newtonsoft.Json;
using MaerskScheduleBot;

internal class Program
{
    private static void Main()
    {
        
        string activeVesselsJsonPath = Path.Combine(Environment.CurrentDirectory, @"Resources\active.json");
        VesselsManager vesselsManager = JsonConvert.DeserializeObject<VesselsManager>(System.IO.File.ReadAllText(activeVesselsJsonPath));
        TelegramBotClient telegramClient = new("5527446858:AAFnOJ4Y0w3a9ZB-cu4WtVMZEssBNyRLEPU");
        TelegramHandler telegramHandler = new(telegramClient, vesselsManager);
        telegramHandler.client.StartReceiving(telegramHandler.TelegramUpdate, telegramHandler.TelegramError);
        Console.ReadLine();
    }
}