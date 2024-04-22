namespace Blog.Domain.Entities
{
    public partial class PostMeta
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Content { get; set; }
        
        // Foreign key property
        public Guid PostId { get; set; }
        // Navigation property
        public Post Post { get; set; }
    }
}