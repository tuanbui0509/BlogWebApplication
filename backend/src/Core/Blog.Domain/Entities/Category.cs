using Blog.Domain.Common;

namespace Blog.Domain.Entities
{
    public partial class Category : BaseAuditableEntity
    {
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        
        public ICollection<PostCategories> PostCategories { get; set; }
    }
}