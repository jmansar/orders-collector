namespace OrdersCollector.DAL
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
    }
}
