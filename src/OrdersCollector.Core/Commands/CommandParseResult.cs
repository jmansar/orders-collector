namespace OrdersCollector.Core.Commands
{
    public class CommandParseResult
    {
        public bool IsCommand { get; set; }
        public CommandInfo CommandInfo { get; set; }

        public CommandParseResult()
        {
            
        }

        public CommandParseResult(CommandInfo commandInfo)
        {
            IsCommand = true;
            CommandInfo = commandInfo;
        }
    }
}
