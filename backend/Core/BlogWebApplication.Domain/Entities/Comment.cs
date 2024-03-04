using BlogWebApplication.Domain.Common;

namespace BlogWebApplication.Domain.Entities
{
    public partial class Comment : BaseAuditableEntity
    {
        public string CommentContents { get; set; }
        public string PostedBy { get; set; }
        public Comment ParentComment { get; set; }
        public int? ParentCommentId { get; set; }
        public virtual ICollection<Comment> ChildComments { get; } = new HashSet<Comment>();
        public Post ParentPost { get; set; }
        public int PostId { get; set; }
    }
}