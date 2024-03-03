using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogWebApplication.Application.Interfaces.Repositories;
using BlogWebApplication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogWebApplication.Persistence.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly IGenericRepository<Post> _repository;

        public PlayerRepository(IGenericRepository<Post> repository) 
        {
            _repository = repository;
        }

        public async Task<List<Post>> GetPlayersByClubAsync(int clubId)
        {
            return await _repository.Entities.Where(x => x.ClubId == clubId).ToListAsync();
        }
    }
}