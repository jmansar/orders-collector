using OrdersCollector.Core.Models;

namespace OrdersCollector.Core.Commands
{
    /// <summary>
    /// Base command.
    /// </summary>
    /// <typeparam name="TResult">Execution result type.</typeparam>
    public abstract class Command<TResult> : ICommand<TResult> where TResult : ICommandResult
    {
        protected readonly ICommandEventsManager CommandEventsManager;

        protected Command(ICommandEventsManager commandEventsManager)
        {
            CommandEventsManager = commandEventsManager;
        }


        /// <summary>
        /// Executes the command.
        /// </summary>
        public virtual void Execute()
        {
            CommandEventsManager.RaiseCommandExecuted(this);
        }

        /// <summary>
        /// Sets command arguments.
        /// </summary>
        /// <param name="args">Arguments.</param>
        public virtual void SetArgs(params string[] args)
        {
        }

        /// <summary>
        /// Result of the execution.
        /// </summary>
        public ICommandResult Result
        {
            get
            {
                return TypedResult;
            }
        }

        /// <summary>
        /// Result of the execution.
        /// </summary>
        public TResult TypedResult { get; set; }

        /// <summary>
        /// User and source of the command.
        /// </summary>
        public AuditInfo AuditInfo { get; set; }
    }
}
