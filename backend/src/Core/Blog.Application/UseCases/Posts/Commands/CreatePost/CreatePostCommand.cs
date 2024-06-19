using Blog.Application.Common.Mappings;
using Blog.Domain.Entities;
using MediatR;

namespace Blog.Application.UseCases.Posts.Commands.CreatePost
{
    public class CreatePostCommand : IRequest<Guid>, IMapFrom<Post>
    {
        public string Title { get; set; }
        public string PostContents { get; set; }
        public string Slug { get; set; }
    }
}