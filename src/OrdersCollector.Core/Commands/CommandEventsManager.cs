using System;

namespace OrdersCollector.Core.Commands
{
    /// <summary>
    /// Provides interface that allows to register command events handlers
    /// </summary>
    public interface ICommandEventsManager
    {
        event Action<ICommand> CommandExecuted;

        void RaiseCommandExecuted(ICommand command);
    }

    /// <summary>
    /// Emits command events.
    /// </summary>
    public class CommandEventsManager : ICommandEventsManager
    {
        public event Action<ICommand> CommandExecuted;

        public void RaiseCommandExecuted(ICommand command)
        {
            if (CommandExecuted != null)
                CommandExecuted(command);
        }
    }
}
