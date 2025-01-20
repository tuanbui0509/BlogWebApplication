using System.Text.Json;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Application.Business.Posts.Queries.GetPostById;
using Blog.Application.Common.Interfaces.Repositories;
using Blog.Domain.Entities;
using Blog.Shared.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Blog.Application.Business.Posts.Handlers
{
    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, TResult<GetPostByIdDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public GetPostByIdQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, IDistributedCache cache = null)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task<TResult<GetPostByIdDto>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var cachedPost = await _cache.GetStringAsync($"Post:{request.PostId}");

            if (cachedPost != null)
            {
                return new DataResult<GetPostByIdDto>(JsonSerializer.Deserialize<GetPostByIdDto>(cachedPost), $"Post:{request.PostId}");
            }
            var post = await _unitOfWork.Repository<Post>().Entities
                   .ProjectTo<GetPostByIdDto>(_mapper.ConfigurationProvider)
                   .AsNoTracking()
                   .FirstOrDefaultAsync(x => x.Id == request.PostId);
            if (post != null)
            {
                // Cache the post
                await _cache.SetStringAsync($"Post:{request.PostId}", JsonSerializer.Serialize(post));
                return new DataResult<GetPostByIdDto>(post, $"Post:{request.PostId}");
            }
            else
            {
                return new DataResult<GetPostByIdDto>(null, $"Cannot find {request.PostId}", isSuccess: false);
            }
        }
    }
}