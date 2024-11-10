using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blog.Application.Common.Interfaces.Repositories;
using Blog.Domain.Entities;
using Blog.Shared.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Business.Posts.Queries.GetAllPosts
{
    public record GetAllPostsQuery : IRequest<TResult<List<GetAllPostsDto>>>;
    internal class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, TResult<List<GetAllPostsDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllPostsQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<TResult<List<GetAllPostsDto>>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _unitOfWork.Repository<Post>().Entities
                   .ProjectTo<GetAllPostsDto>(_mapper.ConfigurationProvider)
                   .AsNoTracking()
                   .ToListAsync(cancellationToken);
            return new DataResult<List<GetAllPostsDto>>(posts, "Sample found");
        }
    }
}