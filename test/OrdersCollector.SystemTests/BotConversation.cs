using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Connector.DirectLine;

namespace OrdersCollector.SystemTests
{
    public class BotConversation : IDisposable
    {
        private readonly BlockingCollection<string> receivedMessages = new BlockingCollection<string>();
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        private readonly DirectLineClient client;
        private readonly string botId;

        private readonly Conversation conversation;

        public BotConversation(DirectLineClient client, string botId)
        {
            this.client = client;
            this.botId = botId;

            conversation = client.Conversations.StartConversation();

            StartPollingForMessages();
        }

        private void StartPollingForMessages()
        {
            Task.Factory.StartNew(
                () => ReadBotMessagesAsync(cancellationTokenSource.Token),
                cancellationTokenSource.Token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Current);
        }

        public string ReadMessage(TimeSpan timeout)
        {
            receivedMessages.TryTake(out string value, timeout);

            return value;
        }

        public async Task SendMessageAsync(string messageText, TestUser user)
        {
            Activity userMessage = new Activity
            {
                From = new ChannelAccount(user.Id, user.Name),
                Text = messageText,
                Type = ActivityTypes.Message
            };

            var response = await client.Conversations.PostActivityAsync(conversation.ConversationId, userMessage);
        }

        private async Task ReadBotMessagesAsync(CancellationToken cancellationToken)
        {
            string watermark = null;
            string conversationId = conversation.ConversationId;

            while (!cancellationToken.IsCancellationRequested)
            {
                var activitySet = await client.Conversations.GetActivitiesAsync(conversationId, watermark);

                watermark = activitySet?.Watermark;

                var activities = activitySet.Activities
                    .Where(x => x.From.Id == botId);

                foreach (Activity activity in activities)
                {
                    receivedMessages.Add(activity.Text);
                }

                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        public void Dispose()
        {
            cancellationTokenSource.Cancel();
        }
    }
}
