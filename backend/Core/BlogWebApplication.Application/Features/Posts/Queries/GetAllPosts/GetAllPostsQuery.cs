using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlogWebApplication.Application.Interfaces.Repositories;
using BlogWebApplication.Domain.Entities;
using BlogWebApplication.Shared.Implements;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogWebApplication.Application.Features.Posts.Queries.GetAllPosts
{
    public record GetAllPostsQuery : IRequest<Result<List<GetAllPostsDto>>>;

    internal class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, Result<List<GetAllPostsDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllPostsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllPostsDto>>> Handle(GetAllPostsQuery query, CancellationToken cancellationToken)
        { 
            var Posts = await _unitOfWork.Repository<Post>().Entities
                   .ProjectTo<GetAllPostsDto>(_mapper.ConfigurationProvider)
                   .ToListAsync(cancellationToken);

            return await Result<List<GetAllPostsDto>>.SuccessAsync(Posts);
        }
    }
}