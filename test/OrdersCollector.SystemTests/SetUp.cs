using System;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace OrdersCollector.SystemTests
{
    [SetUpFixture]
    public class SetUpFixture
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("testsettings.json", optional: false)
                .Build();

            var directLineSecret = Environment.GetEnvironmentVariable("DirectLineSecret") ?? configuration["DirectLineSecret"];
            var botId = Environment.GetEnvironmentVariable("BotId") ?? configuration["BotId"];

            BotClient.Initialize(directLineSecret, botId);
        }
    }
}
