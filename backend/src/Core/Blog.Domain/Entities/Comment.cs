using Blog.Domain.Common;
using Blog.Domain.Identity;

namespace Blog.Domain.Entities
{
    public partial class Comment : BaseAuditableEntity
    {
        public string CommentContents { get; set; }

        // Foreign key properties
        public Guid? ParentCommentId { get; set; }
        public Guid PostId { get; set; }
        public string UserId { get; set; }

        // Navigation properties
        public Comment ParentComment { get; set; }
        public Post ParentPost { get; set; }
        public ApplicationUser User { get; set; }

        // Child comments
        public ICollection<Comment> ChildComments { get; set; }
    }
}