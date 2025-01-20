using Blog.Domain.Common;
using Blog.Domain.Enums;
using Blog.Domain.Identity;

namespace Blog.Domain.Entities
{
    public partial class Post : BaseAuditableEntity
    {
        public string Title { get; set; }
        public string PostContents { get; set; }
        public string Slug { get; set; }
        public PublishState IsPublished { get; set; } = PublishState.Draft;
        public Guid AuthorId { get; set; }
        // Foreign key properties

        // Navigation properties
        public ApplicationUser Author { get; set; }
        public ICollection<PostCategories> PostCategories { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<PostMeta> PostMetas { get; set; }
        public ICollection<PostTag> PostTags { get; set; }
    }
}