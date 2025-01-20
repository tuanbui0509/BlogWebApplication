using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        #region Add DbSet
        public DbSet<Post> Posts { get; }
        public DbSet<Category> Categories { get; }
        public DbSet<PostCategories> PostCategories { get; }
        public DbSet<Tag> Tags { get; }
        public DbSet<PostTag> PostTags { get; }
        public DbSet<Comment> Comments { get; }
        public DbSet<Like> Likes { get; }
        public DbSet<PostMeta> PostMetas { get; }
        public DbSet<RefreshToken> RefreshTokens { get; }

        #endregion

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}