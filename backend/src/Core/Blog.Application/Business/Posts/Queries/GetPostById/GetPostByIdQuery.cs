using Blog.Shared.Result;
using MediatR;

namespace Blog.Application.Business.Posts.Queries.GetPostById
{
    public class GetPostByIdQuery : IRequest<TResult<GetPostByIdDto>>
    {
        public GetPostByIdQuery(Guid postId)
        {
            PostId = postId;
        }

        public Guid PostId { get; }
    }
}