using System.Threading.Tasks;
using AutoFixture;

namespace OrdersCollector.SystemTests.Fixtures
{
    public class OrderFixture
    {
        private IFixture fixture;

        public OrderFixture()
        {
            fixture = new Fixture();
        }

        public async Task AddOrderItemAsync(
            BotConversation conversation, 
            string supplierName, 
            string orderText = null,
            TestUser user = null)
        {
            orderText = orderText ?? fixture.Create<string>();
            user = user ?? fixture.Create<TestUser>();

            var response = await CommandExecutor.ExecuteAndWaitForResponseAsync(
                conversation,
                $"[{supplierName}] {orderText}",
                user);

            if (!response.Contains($"'{user.Name}' -> '{supplierName}'"))
            {
                throw new OperationFailedException(
                    $"Failed to add order item. Supplier: {supplierName}, User: {user.Name}, Order text: {orderText}. Received response: {response}");
            }
        }

        public async Task PickRandomOperatorAsync(
            BotConversation conversation,
            string supplierName,
            TestUser user = null)
        {
            user = user ?? fixture.Create<TestUser>();

            var response = await CommandExecutor.ExecuteAndWaitForResponseAsync(
                conversation,
                $"#losuj {supplierName}",
                user);

            if (!response.Contains("Dokonano losowania"))
            {
                throw new OperationFailedException(
                    $"Failed to pick random operator. Supplier: {supplierName}, User: {user.Name}. Received response: {response}");
            }
        }
    }
}
