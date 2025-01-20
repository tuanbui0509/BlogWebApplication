using Blog.Application.Common.Mappings;
using Blog.Application.Dtos.Post;
using Blog.Domain.Entities;

namespace Blog.Application.Business.Posts.Queries.GetAllPosts
{
    public class GetAllPostsDto : PostDto, IMapFrom<Post>
    {
        // Foreign key properties
        public string AuthorId { get; init; }
    }
}