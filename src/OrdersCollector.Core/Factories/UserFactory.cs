using OrdersCollector.DAL;
using OrdersCollector.Core.Models;

namespace OrdersCollector.Core.Factories
{

    public interface IUserFactory : IFactory<User>
    {
    }

    public class UserFactory : FactoryBase<User>, IUserFactory
    {
        public override User Create()
        {
            var user = base.Create();

            user.CanBeOperator = true;
            user.IsActive = true;

            return user;
        }
    }
}
