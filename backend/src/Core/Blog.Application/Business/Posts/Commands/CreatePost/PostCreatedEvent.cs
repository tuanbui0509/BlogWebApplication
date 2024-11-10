using Blog.Domain.Common;
using Blog.Domain.Entities;

namespace Blog.Application.Business.Posts.Commands.CreatePost
{
    public class PostCreatedEvent : BaseEvent
    {
        public Post Post { get; }

        public PostCreatedEvent(Post post)
        {
            Post = post;
        }
    }
}