using Blog.Application.Common.Interfaces.Repositories;
using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.Persistence.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly IGenericRepository<Post> _repository;

        public PostRepository(IGenericRepository<Post> repository)
        {
            _repository = repository;
        }

        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            return await _repository.Entities.Where(x => x.Id == postId).FirstOrDefaultAsync();
        }
    }
}