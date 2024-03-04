using AutoMapper;
using BlogWebApplication.Application.Interfaces.Repositories;
using BlogWebApplication.Domain.Entities;
using BlogWebApplication.Shared.Implements;
using MediatR;

namespace BlogWebApplication.Application.Features.Posts.Queries.GetPostById
{
    public record GetPostByIdQuery : IRequest<Result<GetPostByIdDto>>
    {
        public Guid Id { get; set; }

        public GetPostByIdQuery()
        {

        }

        public GetPostByIdQuery(Guid id)
        {
            Id = id;
        }
    }

    internal class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, Result<GetPostByIdDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPostByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetPostByIdDto>> Handle(GetPostByIdQuery query, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<Post>().GetByIdAsync(query.Id);
            var Post = _mapper.Map<GetPostByIdDto>(entity);
            return await Result<GetPostByIdDto>.SuccessAsync(Post);
        }
    }
}