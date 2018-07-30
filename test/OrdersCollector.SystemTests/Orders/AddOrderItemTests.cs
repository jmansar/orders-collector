using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using OrdersCollector.SystemTests.Fixtures;

namespace OrdersCollector.SystemTests.Orders
{
    [TestFixture]
    public class AddOrderItemTests
    {
        private IFixture fixture;
        private SupplierFixture supplierFixture;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            fixture = new Fixture();
            supplierFixture = new SupplierFixture();
        }

        [Test]
        public async Task CanAddOrderItem()
        {
            // Arrange
            var orderText = fixture.Create<string>();
            var user = fixture.Create<TestUser>();

            using (var conversation = BotClient.Instance.CreateConversation())
            {
                var supplierName = await supplierFixture.CreateSupplierAsync(conversation);

                // Act
                var response = await CommandExecutor.ExecuteAndWaitForResponseAsync(
                    conversation,
                    $"[{supplierName}] {orderText}",
                    user);

                // Assert
                Assert.That(response, Contains.Substring($"'{user.Name}' -> '{supplierName}'"));
            }
        }
    }
}
