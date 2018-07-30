using OrdersCollector.Core.Models;

namespace OrdersCollector.Core.Commands
{
    /// <summary>
    /// Interface of the command.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        void Execute();

        /// <summary>
        /// User and source of the command.
        /// </summary>
        AuditInfo AuditInfo { get; set; }

        /// <summary>
        /// Result of the execution.
        /// </summary>
        ICommandResult Result { get; }

        /// <summary>
        /// Set command arguments.
        /// </summary>
        /// <param name="args">Arguments.</param>
        void SetArgs(params string[] args);
    }

    /// <summary>
    /// Interface of the command.
    /// </summary>
    /// <typeparam name="TResult">Result of the command execution.</typeparam>
    public interface ICommand<TResult> : ICommand where TResult : ICommandResult
    {
        /// <summary>
        /// Result of the execution.
        /// </summary>
        TResult TypedResult { get; }
    }
}
