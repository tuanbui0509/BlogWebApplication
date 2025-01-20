using System.Text.Json;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Application.Business.Posts.Queries.GetAllPosts;
using Blog.Application.Common.Interfaces.Repositories;
using Blog.Domain.Entities;
using Blog.Shared.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Blog.Application.Business.Posts.Handlers
{
    public class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, PaginatedResult<GetAllPostsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public GetAllPostsQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, IDistributedCache cache = null)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task<PaginatedResult<GetAllPostsDto>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            if (request.PageNumber <= 0 || request.PageSize <= 0)
                return PaginatedResult<GetAllPostsDto>.Failure("Invalid pagination parameters");

            var cachePosts = await _cache.GetStringAsync("Posts");
            List<GetAllPostsDto> posts;
            if (cachePosts != null)
            {
                posts = JsonSerializer.Deserialize<List<GetAllPostsDto>>(cachePosts);
                if (posts == null)
                    return PaginatedResult<GetAllPostsDto>.Failure("Error deserializing posts from cache");

                posts = posts
                .OrderByDescending(x => x.UpdatedDate)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize).ToList();

                var totalItems = posts.Count();
                return PaginatedResult<GetAllPostsDto>.Successful(posts, request.PageNumber, totalItems, request.PageSize);
            }

            posts = await _unitOfWork.Repository<Post>()
                            .Entities
                            .AsQueryable()
                            .Include(post => post.PostTags)
                            .ThenInclude(postTag => postTag.Tag) // Navigate through PostTags to Tags
                            .ProjectTo<GetAllPostsDto>(_mapper.ConfigurationProvider)
                            .AsNoTracking()
                            .OrderByDescending(post => post.UpdatedDate)
                            .ToListAsync(cancellationToken);

            // Cache the post
            await _cache.SetStringAsync($"Posts", JsonSerializer.Serialize(posts));
            // Paging
            posts = posts
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize).ToList();

            return PaginatedResult<GetAllPostsDto>.Successful(posts, request.PageNumber, posts.Count(), request.PageSize);
        }
    }
}