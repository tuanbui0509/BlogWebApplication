using BlogWebApplication.Domain.Common;

namespace BlogWebApplication.Domain.Entities
{
    public class Tag : BaseAuditableEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public ICollection<PostTags> PostTags { get; set; } = new List<PostTags>();
    }
}