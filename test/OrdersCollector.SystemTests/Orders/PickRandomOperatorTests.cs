using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using OrdersCollector.SystemTests.Fixtures;

namespace OrdersCollector.SystemTests.Orders
{
    [TestFixture]
    public class PickRandomOperatorTests
    {
        private Fixture fixture;
        private SupplierFixture supplierFixture;
        private OrderFixture orderFixture;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            fixture = new Fixture();
            supplierFixture = new SupplierFixture();
            orderFixture = new OrderFixture();
        }

        [Test]
        public async Task CanPickOrderOperator()
        {
            // Arrange
            const int orderItemCount = 3;
            var users = fixture.CreateMany<TestUser>(orderItemCount).ToArray();

            using (var conversation = BotClient.Instance.CreateConversation())
            {
                var supplierName = await supplierFixture.CreateSupplierAsync(conversation);

                for (var i = 0; i < orderItemCount; i++)
                {
                    await orderFixture.AddOrderItemAsync(conversation, supplierName, user: users[i]);
                }

                // Act
                var response = await CommandExecutor.ExecuteAndWaitForResponseAsync(
                    conversation,
                    $"#losuj {supplierName}",
                    users.First());

                // Assert
                var pattern = $@"Operatorem zamówienia do '{supplierName}'  z dnia (.*)  zostaje \*\*({String.Join("|", users.Select(u => u.Name))})\*\*";
                StringAssert.IsMatch(pattern, response);

                Assert.That(response, Contains.Substring($"Dokonano losowania wśród ({String.Join(", ", users.Select(u => u.Name))})."));
            }
        }
    }
}
