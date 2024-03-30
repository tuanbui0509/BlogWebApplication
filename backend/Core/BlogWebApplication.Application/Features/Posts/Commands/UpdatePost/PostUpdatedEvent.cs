using BlogWebApplication.Domain.Common;
using BlogWebApplication.Domain.Entities;

namespace BlogWebApplication.Application.Features.Posts.Commands.UpdatePost
{
    public class PostUpdatedEvent: BaseEvent
    {
        public Post Post { get; }

        public PostUpdatedEvent(Post Post)
        {
            Post = Post;
        }
    }
}