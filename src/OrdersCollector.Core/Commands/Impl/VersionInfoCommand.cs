using System;
using System.Reflection;
using OrdersCollector.Resources;

namespace OrdersCollector.Core.Commands.Impl
{
    public class VersionInfoCommand : Command<VersionInfoResult>
    {
        public VersionInfoCommand(ICommandEventsManager commandEventsManager) : base(commandEventsManager)
        {
        }

        public override void Execute()
        {
            TypedResult = new VersionInfoResult()
            {
                Version = Assembly.GetExecutingAssembly()
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                    .InformationalVersion
                    .ToString()
            };
            base.Execute();
        }
    }

    public class VersionInfoResult : CommandResult
    {
        public string Version { get; set; }
        public override string GetMessage()
        {
            return String.Format(Messages.VersionInfo, Version);
        }
    }
}
