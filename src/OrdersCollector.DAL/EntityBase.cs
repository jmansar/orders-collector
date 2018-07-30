namespace OrdersCollector.DAL
{
    /// <summary>
    /// Base class for entities.
    /// </summary>
    /// <typeparam name="TKey">Type of entity identifier.</typeparam>
    public abstract class EntityBase<TKey> where TKey : struct
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public virtual TKey Id { get; set; }

        /// <summary>
        /// Returns true if entity is transient.
        /// </summary>
        public virtual bool IsTransient()
        {
            return Id.Equals(default(TKey));
        }
    }
}
