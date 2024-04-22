using Blog.Domain.Identity;

namespace Blog.Domain.Entities
{
    public class Like
    {
        public int LikeId { get; set; }

        // Foreign key property
        public string UserId { get; set; }
        public Guid PostId { get; set; }
        // Navigation property
        public ApplicationUser ApplicationUser { get; set; }
        public Post Post { get; set; }
    }
}