namespace OrdersCollector.Core.Commands
{
    /// <summary>
    /// Interface of the command result.
    /// </summary>
    public interface ICommandResult
    {
        /// <summary>
        /// Returns text result message that can be presented to the end user.
        /// </summary>
        string GetMessage();
    }

    /// <summary>
    /// Base class for Command results.
    /// </summary>
    public abstract class CommandResult : ICommandResult
    {
        /// <summary>
        /// Returns text result message that can be presented to the end user.
        /// </summary>
        public abstract string GetMessage();
    }
}
