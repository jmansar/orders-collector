using System;
using OrdersCollector.Core.Models;

namespace OrdersCollector.Core.Commands
{
    public interface ICommandFactory
    {
        ICommand Create(CommandInfo commandInfo, AuditInfo auditInfo);
    }

    public class CommandFactory : ICommandFactory
    {
        private readonly Func<string, ICommand> commandFactoryFunc;

        public CommandFactory(Func<string, ICommand> commandFactoryFunc)
        {
            this.commandFactoryFunc = commandFactoryFunc;
        }

        public ICommand Create(CommandInfo commandInfo, AuditInfo auditInfo)
        {
            var command = commandFactoryFunc(commandInfo.Name);
            if (command == null)
            {
                throw new AppException(ErrorCodes.Common.UnknownCommand);
            }

            command.SetArgs(commandInfo.Arguments);
            command.AuditInfo = auditInfo;

            return command;
        }
    }
}
