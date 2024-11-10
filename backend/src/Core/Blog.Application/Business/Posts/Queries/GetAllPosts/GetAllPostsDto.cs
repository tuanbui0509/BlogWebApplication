using Blog.Application.Common.Mappings;
using Blog.Domain.Entities;

namespace Blog.Application.Business.Posts.Queries.GetAllPosts
{
    public class GetAllPostsDto : IMapFrom<Post>
    {
        public Guid Id { get; init; }
        public string? Title { get; init; }
        public string? PostContents { get; init; }
        public string? Slug { get; init; }

        // Foreign key properties
        public string? UserId { get; init; }
    }
}