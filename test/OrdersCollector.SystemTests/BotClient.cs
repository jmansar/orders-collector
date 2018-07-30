using Microsoft.Bot.Connector.DirectLine;

namespace OrdersCollector.SystemTests
{
    public class BotClient
    {
        private static BotClient instance;

        public static BotClient Instance => instance;

        public static void Initialize(string directLineSecret, string botId)
        {
            instance = new BotClient(directLineSecret, botId);
        }

        private readonly string botId;
        private readonly DirectLineClient client;

        public BotClient(string directLineSecret, string botId)
        {
            this.botId = botId;

            client = new DirectLineClient(directLineSecret);
        }

        public BotConversation CreateConversation()
        {
            return new BotConversation(client, botId);
        }
    }
}
