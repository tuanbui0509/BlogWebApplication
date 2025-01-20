using Blog.Domain.Entities;

namespace Blog.Application.Abstractions
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken refreshToken);
        Task<RefreshToken> GetByTokenAsync(string token);
        Task UpdateAsync(RefreshToken refreshToken);
    }
}