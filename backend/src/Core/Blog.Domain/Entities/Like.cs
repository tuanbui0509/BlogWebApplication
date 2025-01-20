using Blog.Domain.Common;
using Blog.Domain.Identity;

namespace Blog.Domain.Entities
{
    public partial class Like : BaseAuditableEntity
    {
        // Foreign key property
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        // Navigation property
        public ApplicationUser User { get; set; }
        public Post Post { get; set; }
    }
}