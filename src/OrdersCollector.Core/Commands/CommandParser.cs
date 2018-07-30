using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace OrdersCollector.Core.Commands
{
    /// <summary>
    /// Provides method of parsing user text requests.
    /// </summary>
    public interface ICommandParser
    {
        /// <summary>
        /// Returns command info object based on the user text input.
        /// </summary>
        CommandParseResult Parse(string text);
    }

    public class CommandParser : ICommandParser
    {
        private Regex orderPattern;
        private Regex commandPattern;

        public CommandParser()
        {
            orderPattern = new Regex("^\\[(?<name>[\\w\\s]*)\\](?<content>.*)");
            commandPattern = new Regex(@"^#(?<name>[a-zA-Z0-9]+)(\s*)(?<args>.*)");
        }

        public CommandParseResult Parse(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                return new CommandParseResult();
            }

            text = SanitizeInput(text);

            var orderCommandResult = ParseOrderCommand(text);
            if (orderCommandResult.IsCommand) return orderCommandResult;


            return ParseStandardCommand(text);
        }

        private CommandParseResult ParseOrderCommand(string text)
        {
            var match = orderPattern.Match(text);
            if (match.Success)
            {
                var orderSupplier = match.Groups["name"].Value.Trim();
                var orderContent = match.Groups["content"].Value.Trim();

                return new CommandParseResult(new CommandInfo()
                {
                    Name = "dodaj",
                    Arguments = new[] {orderSupplier, "", orderContent}
                });

            }

            return new CommandParseResult();
        }

        private static string SanitizeInput(string text)
        {
            return text.Trim();
        }

        private CommandParseResult ParseStandardCommand(string text)
        {
            var match = commandPattern.Match(text);
            if (match.Success)
            {
                var commandInfo = new CommandInfo()
                {
                    Name = GetStandardCommandName(match),
                    Arguments = GetStandardCommandArguments(match)
                };


                return new CommandParseResult(commandInfo);
            }

            return new CommandParseResult();
        }

        private static string GetStandardCommandName(Match match)
        {
            return match.Groups["name"].Value.Trim();
        }

        private string[] GetStandardCommandArguments(Match match)
        {
            var argsString = match.Groups["args"].Value.Trim();
            return String.IsNullOrWhiteSpace(argsString) ? new string[0] { } : argsString.Split(',').Select(x => x.Trim()).ToArray();
        }
    }
}
