using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace OrdersCollector.DAL.EF
{
    /// <summary>
    /// Base repository class Entity Framework.
    /// </summary>
    /// <typeparam name="TEntity">Entity.</typeparam>
    /// <typeparam name="TKey">Entity identifier type.</typeparam>
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : EntityBase<TKey> where TKey: struct
    {
        private readonly IContextProvider contextProvider;

        protected DbSet<TEntity> DbSet
        {
            get
            {
                return contextProvider.GetCurrentContext().Set<TEntity>();
            }
        }

        protected DbContext Context
        {
            get
            {
                return contextProvider.GetCurrentContext();
            }
        }

        public Repository(IContextProvider contextProvider)
        {
            this.contextProvider = contextProvider;
        }

        /// <summary>
        /// Returns entity with specicifed ID.
        /// </summary>
        /// <param name="id">Id.</param>
        public TEntity GetById(TKey id)
        {
            return DbSet.Find(id);
        }

        /// <summary>
        /// Returns IQueryable.
        /// </summary>
        public IQueryable<TEntity> AsQueryable()
        {
            return DbSet;
        }

        /// <summary>
        /// Adds new entity instance.
        /// </summary>
        public void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        /// <summary>
        /// Deletes entity instance.
        /// </summary>
        public void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }
    }
}
