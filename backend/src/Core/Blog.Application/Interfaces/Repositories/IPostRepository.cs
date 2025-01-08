using Blog.Domain.Entities;

namespace Blog.Application.Common.Interfaces.Repositories
{
    public interface IPostRepository
    {
        Task<Post> GetPostByIdAsync(Guid postId);
    }
}