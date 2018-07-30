using OrdersCollector.Resources;

namespace OrdersCollector.Core.Commands.Impl
{
    public class HelpCommand : Command<HelpResult>
    {
        public HelpCommand(ICommandEventsManager commandEventsManager) : base(commandEventsManager)
        {
        }

        public override void Execute()
        {
            TypedResult = new HelpResult();
            base.Execute();
        }
    }

    public class HelpResult : CommandResult
    {
        public override string GetMessage()
        {
            return Messages.HelpInfo;
        }
    }
}
