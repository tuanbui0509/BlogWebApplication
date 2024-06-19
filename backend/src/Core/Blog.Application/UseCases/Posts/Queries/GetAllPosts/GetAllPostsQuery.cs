using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Application.Common.Interfaces.Repositories;
using Blog.Domain.Entities;
using Blog.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.UseCases.Posts.Queries.GetAllPosts
{
    public record GetAllPostsQuery : IRequest<Result<List<GetAllPostsDto>>>;
    internal class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, Result<List<GetAllPostsDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllPostsQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<GetAllPostsDto>>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _unitOfWork.Repository<Post>().Entities
                   .ProjectTo<GetAllPostsDto>(_mapper.ConfigurationProvider)
                   .AsNoTracking()
                   .ToListAsync(cancellationToken);
            return await Result<List<GetAllPostsDto>>.SuccessAsync(posts);
        }
    }
}