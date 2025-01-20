using Blog.Domain.Common;

namespace Blog.Domain.Entities
{
    public partial class Tag : BaseAuditableEntity
    {
        public string TagName { get; set; }
        public string Slug { get; set; }
        public ICollection<PostTag> PostTags { get; set; }
    }
}