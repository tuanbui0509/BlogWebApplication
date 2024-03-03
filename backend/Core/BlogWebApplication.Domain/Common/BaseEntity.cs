using System.ComponentModel.DataAnnotations.Schema;
using BlogWebApplication.Domain.Common.Interfaces;

namespace BlogWebApplication.Domain.Common
{
    public class BaseEntity : IEntity
    {
        private readonly List<BaseEvent> _domainEvents = new();
        
        public Guid Id { get; set; }

        [NotMapped]
        public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(BaseEvent domainEvent) => _domainEvents.Add(domainEvent);

        public void RemoveDomainEvent(BaseEvent domainEvent) => _domainEvents.Remove(domainEvent);

        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}