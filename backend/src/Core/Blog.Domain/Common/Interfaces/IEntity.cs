namespace Blog.Domain.Common.Interfaces
{
    /// <summary>
    /// This is the base interface for all entities
    /// </summary>
    public interface IEntity;
    
    /// <summary>
    /// This is the base interface for all entities with a key.
    /// </summary>
    /// <typeparam name="TKey">The type of the entity key.</typeparam>
    public interface IEntity<out TKey> : IEntity where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets the ID of the entity.
        /// </summary>
        TKey Id { get; }
    }
}