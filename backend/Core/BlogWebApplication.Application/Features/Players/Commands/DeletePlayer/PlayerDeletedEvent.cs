using BlogWebApplication.Domain.Common;
using BlogWebApplication.Domain.Entities;

namespace BlogWebApplication.Application.Features.Posts.Commands.DeletePost
{
    public class PostDeletedEvent : BaseEvent
    {
        public Post Post { get; }

        public PostDeletedEvent(Post Post)
        {
            Post = Post;
        }
    }
}