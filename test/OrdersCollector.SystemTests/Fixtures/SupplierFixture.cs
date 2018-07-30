using System.Threading.Tasks;
using AutoFixture;

namespace OrdersCollector.SystemTests.Fixtures
{
    public class SupplierFixture
    {
        private IFixture fixture;

        public SupplierFixture()
        {
            fixture = new Fixture();
        }

        public async Task<string> CreateSupplierAsync(BotConversation conversation, string supplierName = null)
        {
            supplierName = fixture.Create<string>().Replace("-", "");

            var response = await CommandExecutor.ExecuteAndWaitForResponseAsync(
                conversation,
                $"#nowyDostawca {supplierName}",
                fixture.Create<TestUser>());

            if (response != $"Nowy dostawca: '{supplierName}'")
            {
                throw new OperationFailedException($"Failed to create supplier {supplierName}. Received response: {response}");
            }

            return supplierName;
        }
    }
}
