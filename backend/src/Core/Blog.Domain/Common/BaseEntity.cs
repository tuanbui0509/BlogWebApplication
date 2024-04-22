using Blog.Domain.Common.Interfaces;

namespace Blog.Domain.Common
{
    public class BaseEntity : IEntity
    {
        public Guid Id { get; set; }
    }
}