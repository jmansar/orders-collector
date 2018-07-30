using System;
using System.Threading.Tasks;
using OrdersCollector.SystemTests.Fixtures;

namespace OrdersCollector.SystemTests
{
    public static class CommandExecutor
    {
        public static async Task<string> ExecuteAndWaitForResponseAsync(BotConversation conversation, string commandText, TestUser user, TimeSpan? timeout = null)
        {
            await conversation.SendMessageAsync(commandText, user);

            var appliedTimeout = timeout ?? Config.DefaultOperationTimeout;

            var response = conversation.ReadMessage(appliedTimeout);
            if (response == null)
            {
                throw new OperationTimeoutException($"No response for command '{commandText}' received within configured time period {appliedTimeout}");
            }

            return response;
        }
    }
}
