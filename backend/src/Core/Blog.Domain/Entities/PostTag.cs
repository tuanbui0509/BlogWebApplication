using Blog.Domain.Common;

namespace Blog.Domain.Entities
{
    public partial class PostTag :BaseAuditableEntity
    {
        // Foreign key property
        public Guid PostId { get; set; }
        public Guid TagId { get; set; }
        // Navigation property
        public Post Post { get; set; }
        public Tag Tag { get; set; }
    }
}