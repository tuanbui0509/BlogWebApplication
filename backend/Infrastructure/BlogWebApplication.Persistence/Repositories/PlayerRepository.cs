using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogWebApplication.Application.Interfaces.Repositories;
using BlogWebApplication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogWebApplication.Persistence.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly IGenericRepository<Post> _repository;

        public PostRepository(IGenericRepository<Post> repository) 
        {
            _repository = repository;
        }
    }
}