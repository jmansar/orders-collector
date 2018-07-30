using System.Linq;

namespace OrdersCollector.DAL
{
    /// <summary>
    /// Repository interface
    /// </summary>
    public interface IRepository<TEntity, TKey>
        where TEntity : EntityBase<TKey>
        where TKey : struct
    {
        /// <summary>
        /// Returns entity with specicifed ID.
        /// </summary>
        /// <param name="id">Id.</param>
        TEntity GetById(TKey id);

        /// <summary>
        /// Returns IQueryable.
        /// </summary>
        IQueryable<TEntity> AsQueryable();

        /// <summary>
        /// Adds new entity instance.
        /// </summary>
        void Add(TEntity entity);

        /// <summary>
        /// Deletes entity instance.
        /// </summary>
        void Delete(TEntity entity);
    }
}
