using System;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace TelegramBot
{
    class Program
    {
        private static TelegramBotClient botClient;

        static void Main()
        {
            botClient = new TelegramBotClient("6008124704:AAGf0ttePy0HKso96_9TGGsG_h04gxEvOxA");
            botClient.OnMessage += BotClient_OnMessage;
            botClient.StartReceiving();
            Console.WriteLine("Bot started. Press any key to exit.");
            Console.ReadKey();
            botClient.StopReceiving();
        }

        private static async void BotClient_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text == "/start")
            {
                var chatId = e.Message.Chat.Id;
                var username = e.Message.Chat.Username;

                string greeting = $"Hello, {username}! Welcome to the bot.";
                string instructions = "Here are some available commands:\n" +
                                      "/start - Show a greeting\n" +
                                      "/latest_news [topic] - Search for latest news on a topic (up to 5 links)\n" +
                                      "/save_news [url] - Save a news URL\n" +
                                      "/saved_news - Show a list of saved news";

                await botClient.SendTextMessageAsync(chatId, greeting);
                await botClient.SendTextMessageAsync(chatId, instructions);
            }
        }
    }
}
