using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Connector;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrdersCollector.Configuration;
using OrdersCollector.Core;
using OrdersCollector.Core.Commands;
using OrdersCollector.Core.Models;
using OrdersCollector.DAL;

namespace OrdersCollector.Controllers
{
    [Route("api/messages")]
    [BotAuthentication]
    public class MessagesController : Controller
    {
        private readonly ICommandParser commandParser;
        private readonly ICommandFactory commandFactory;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IOptions<AppOption> optionsAccessor;
        private readonly ILogger<MessagesController> log;
        private readonly MicrosoftAppCredentials appCredentials;

        public MessagesController(ICommandParser commandParser, ICommandFactory commandFactory, IUnitOfWorkFactory unitOfWorkFactory, IOptions<AppOption> optionsAccessor, ILogger<MessagesController> log)
        {
            this.commandParser = commandParser;
            this.commandFactory = commandFactory;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.optionsAccessor = optionsAccessor;
            this.log = log;

            appCredentials = new MicrosoftAppCredentials();
        }

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<IActionResult> Post([FromBody]Activity activity)
        {
            log.LogDebug($"Activity received - Type: {activity.Type}");

            if (activity.Type == ActivityTypes.Message)
            {
                log.LogDebug($"Message received: {activity.Text}");

                var replyText = HandleMessageActivity(activity);
                if (!String.IsNullOrWhiteSpace(replyText))
                {
                    var connector = new ConnectorClient(new Uri(activity.ServiceUrl), appCredentials);

                    log.LogDebug($"Reply sent: {replyText}");

                    var reply = activity.CreateReply(replyText);
                    await connector.Conversations.ReplyToActivityAsync(reply);
                }
            }

            return Ok();
        }

        // TODO: extract to separate component
        private string HandleMessageActivity(Activity activity)
        {
            try
            {
                var conversationId = activity.Conversation.Id;
                var allowedConversations = optionsAccessor.Value.AllowedConversations;
                if (allowedConversations != null && !allowedConversations.Contains(conversationId))
                {
                    log.LogWarning($"Conversation {conversationId} is not on the list of allowed conversations");

                    return $"OrdersCollector is not enabled on this conversation. ConversationId = ${conversationId}";
                }

                using (var uow = unitOfWorkFactory.Create())
                {
                    var command = GetCommand(activity);
                    if (command == null)
                    {
                        return null;
                    }

                    command.Execute();
                    uow.Commit();

                    return command.Result.GetMessage();
                }
            }
            catch (AppException appException)
            {
                return appException.Message;
            }
        }

        private ICommand GetCommand(Activity activity)
        {
            var text = GetTextWithoutRecipientMention(activity).Trim();
            var parseResult = commandParser.Parse(text);
            if (parseResult.IsCommand)
            {
                return commandFactory.Create(parseResult.CommandInfo, new AuditInfo()
                {
                    Source = "MsBotFramework",
                    SubSource = activity.Conversation.Id,
                    InvokedBy = activity.From.Id,
                    InvokedByName = activity.From.Name,
                    SubSubSource = activity.Id,
                    Date = activity.Timestamp
                });
            }

            return null;
        }

        private string GetTextWithoutRecipientMention(Activity activity)
        {
            // NOTE: activity.RemoveRecipientMention() wasn't working properly for all clients
            // TODO: check whether it's fixed in the new version
            var text = RemoveFromStart(activity.Text, "@");
            text = RemoveFromStart(text, activity.Recipient.Name);
            text = RemoveFromStart(text, appCredentials.MicrosoftAppId);

            return text;
        }

        private static string RemoveFromStart(string text, string toRemove)
        {
            if (!text.StartsWith(toRemove))
            {
                return text;
            }

            if (text.Length == toRemove.Length)
            {
                return string.Empty;
            }

            return text.Substring(toRemove.Length, text.Length - toRemove.Length);
        }
    }
}
