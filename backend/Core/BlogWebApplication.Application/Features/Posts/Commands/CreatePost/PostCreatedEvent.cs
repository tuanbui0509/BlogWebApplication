using BlogWebApplication.Domain.Common;
using BlogWebApplication.Domain.Entities;

namespace BlogWebApplication.Application.Features.Posts.Commands.CreatePost
{
    public class PostCreatedEvent : BaseEvent
    {
        public Post Post { get; }

        public PostCreatedEvent(Post Post)
        {
            Post = Post;
        }
    }
}