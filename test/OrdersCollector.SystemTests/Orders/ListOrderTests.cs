using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using OrdersCollector.SystemTests.Fixtures;

namespace OrdersCollector.SystemTests.Orders
{
    [TestFixture]
    public class ListOrderTests
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
        public async Task CanListOrderItems()
        {
            // Arrange
            const int orderItemCount = 3;
            var users = fixture.CreateMany<TestUser>(orderItemCount).ToArray();
            var orderTexts = fixture.CreateMany<string>(orderItemCount).ToArray();

            using (var conversation = BotClient.Instance.CreateConversation())
            {
                var supplierName = await supplierFixture.CreateSupplierAsync(conversation);
                
                for (var i = 0; i < orderItemCount; i++)
                {
                    await orderFixture.AddOrderItemAsync(conversation, supplierName, orderTexts[i], users[i]);
                }

                // Act
                var response = await CommandExecutor.ExecuteAndWaitForResponseAsync(
                    conversation,
                    $"#lista {supplierName}",
                    users.First());

                // Assert
                Assert.That(response, Contains.Substring($"Dostawca: **{supplierName}**"));

                for (var i = 0; i < orderItemCount; i++)
                {
                    Assert.That(response, Contains.Substring($"- [{users[i].Name}] **{orderTexts[i]}**"));
                }
            }
        }

        [Test]
        public async Task CanListOrderOperator()
        {
            // Arrange
            var user = fixture.Create<TestUser>();

            using (var conversation = BotClient.Instance.CreateConversation())
            {
                var supplierName = await supplierFixture.CreateSupplierAsync(conversation);
                await orderFixture.AddOrderItemAsync(conversation, supplierName, user: user);
                await orderFixture.PickRandomOperatorAsync(conversation, supplierName, user: user);

                // Act
                var response = await CommandExecutor.ExecuteAndWaitForResponseAsync(
                    conversation,
                    $"#lista {supplierName}",
                    user);

                // Assert
                StringAssert.Contains($"Operator: {user.Name}", response);
            }
        }
    }
}
