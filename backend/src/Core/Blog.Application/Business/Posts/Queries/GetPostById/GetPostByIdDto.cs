using Blog.Application.Common.Mappings;
using Blog.Application.Dtos.Post;
using Blog.Domain.Entities;

namespace Blog.Application.Business.Posts.Queries.GetPostById
{
    public class GetPostByIdDto : PostDto, IMapFrom<Post>
    {

    }
}