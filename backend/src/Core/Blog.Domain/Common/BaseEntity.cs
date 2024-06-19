using System.ComponentModel.DataAnnotations.Schema;
using Blog.Domain.Common.Interfaces;

namespace Blog.Domain.Common
{
    /// <summary>
    /// Represents an abstract base entity class.
    /// </summary>
    public class BaseEntity : IEntity<Guid>
    {
        private readonly List<BaseEvent> _domainEvents = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEntity"/> class.
        /// </summary>
        protected BaseEntity() => Id = Guid.NewGuid();

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEntity"/> class with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        protected BaseEntity(Guid id) => Id = id;
        public Guid Id { get; private init; }

        [NotMapped]
        /// <summary>
        /// Gets the domain events associated with this entity.
        /// </summary>
        public IEnumerable<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

        /// <summary>
        /// Adds a domain event to the entity.
        /// </summary>
        /// <param name="domainEvent">The domain event to add.</param>
        public void AddDomainEvent(BaseEvent domainEvent) => _domainEvents.Add(domainEvent);
        public void RemoveDomainEvent(BaseEvent domainEvent) => _domainEvents.Remove(domainEvent);

        /// <summary>
        /// Clears all the domain events associated with this entity.
        /// </summary>
        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}