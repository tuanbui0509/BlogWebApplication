using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlogWebApplication.Application.Extensions;
using BlogWebApplication.Application.Interfaces.Repositories;
using BlogWebApplication.Domain.Entities;
using BlogWebApplication.Shared.Implements;
using MediatR;

namespace BlogWebApplication.Application.Features.Posts.Queries.GetPostsWithPagination
{
    public record GetPostsWithPaginationQuery : IRequest<PaginatedResult<GetPostsWithPaginationDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetPostsWithPaginationQuery() { }

        public GetPostsWithPaginationQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    internal class GetPostsWithPaginationQueryHandler : IRequestHandler<GetPostsWithPaginationQuery, PaginatedResult<GetPostsWithPaginationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPostsWithPaginationQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<GetPostsWithPaginationDto>> Handle(
            GetPostsWithPaginationQuery query,
            CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Post>().Entities
                   .OrderBy(x => x.Title)
                   .ProjectTo<GetPostsWithPaginationDto>(_mapper.ConfigurationProvider)
                   .ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);
        }
    }
}