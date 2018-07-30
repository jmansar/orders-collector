using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using OrdersCollector.SystemTests.Fixtures;

namespace OrdersCollector.SystemTests.Orders
{
    [TestFixture]
    public class RemoveOrderItem
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
        public async Task CanRemoveMyOrderItem()
        {
            // Arrange
            var user = fixture.Create<TestUser>();

            using (var conversation = BotClient.Instance.CreateConversation())
            {
                var supplierName = await supplierFixture.CreateSupplierAsync(conversation);
                await orderFixture.AddOrderItemAsync(conversation, supplierName, user: user);

                // Act
                var response = await CommandExecutor.ExecuteAndWaitForResponseAsync(
                    conversation,
                    "#usunMojeZam",
                    user);

                // Assert
                StringAssert.Contains("Usunięto pozycję", response);

                var listResponse = await CommandExecutor.ExecuteAndWaitForResponseAsync(
                    conversation,
                    $"#lista {supplierName}",
                    user);

                StringAssert.DoesNotContain(user.Name, listResponse);
            }
        }
    }
}
