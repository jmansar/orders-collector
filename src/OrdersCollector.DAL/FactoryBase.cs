namespace OrdersCollector.DAL
{
    public interface IFactory<T>
    {
        T Create();
    }

    public abstract class FactoryBase<T> : IFactory<T> where T : new()
    {
        public virtual T Create()
        {
            return new T();
        }
    }
}
