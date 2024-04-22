using Blog.Domain.Common;
using Blog.Domain.Identity;

namespace Blog.Domain.Entities
{
    public partial class Post : BaseAuditableEntity
    {
        public string Title { get; set; }
        public string PostContents { get; set; }
        public string Slug { get; set; }
        public string Summary { get; set; }
        
        // Foreign key properties
        public string UserId { get; set; }
        public Guid CategoryId { get; set; }
        
        // Navigation properties
        ApplicationUser ApplicationUser { get; set; }
        public ICollection<PostCategories> PostCategories { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<PostMeta> PostMetas { get; set; }
        public ICollection<PostTag> PostTags { get; set; }
    }
}