using BlogWebApplication.Domain.Common;

namespace BlogWebApplication.Domain.Entities
{
    public partial class Category : BaseAuditableEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public ICollection<Post> AssociatedPosts { get; set; } = new List<Post>();
        public List<PostCategories> PostCategories { get; set; } = new List<PostCategories>();
    }
}