namespace Blog.Domain.Entities
{
    public partial class PostCategories
    {
        public int Id { get; set; }
        
        // Foreign key property
        public Guid PostId { get; set; }
        public Guid CategoryId { get; set; }
        // Navigation property
        public Post Post { get; set; }
        public Category Category { get; set; }
    }
}