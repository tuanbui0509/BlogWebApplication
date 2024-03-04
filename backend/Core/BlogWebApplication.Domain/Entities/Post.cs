using BlogWebApplication.Domain.Common;
using BlogWebApplication.Domain.Entities.Authentication;

namespace BlogWebApplication.Domain.Entities
{
    public partial class Post: BaseAuditableEntity
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Summary { get; set; }
        public string PostContents { get; set; }
        public string AuthorId { get; set; }
        public UserApplication UserApplication { get; set; }
        public ICollection<PostTags> PostTags { get; set; } = new List<PostTags>();
        public ICollection<PostCategories> PostCategories { get; set; } = new List<PostCategories>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<PostMeta> PostMetas { get; set; } = new List<PostMeta>();
    }
}