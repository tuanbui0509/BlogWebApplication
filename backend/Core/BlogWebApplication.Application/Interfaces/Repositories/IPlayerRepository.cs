using BlogWebApplication.Domain.Entities;

namespace BlogWebApplication.Application.Interfaces.Repositories
{
    public interface IPlayerRepository
    {
        Task<List<Post>> GetPlayersByClubAsync(int clubId);
    }
}