using AutoMapper;
using BlogWebApplication.Application.Common.Mappings;
using BlogWebApplication.Application.Interfaces.Repositories;
using BlogWebApplication.Domain.Entities;
using BlogWebApplication.Shared.Implements;
using MediatR;

namespace BlogWebApplication.Application.Features.Posts.Commands.CreatePost
{
    public class CreatePostCommand: IRequest<Result<Guid>>, IMapFrom<Post>
    {
        public string Title { get; set; }
    }

    internal class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreatePostCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper; 
        }

        public async Task<Result<Guid>> Handle(CreatePostCommand command, CancellationToken cancellationToken)
        {
            var Post = new Post()
            {
                Title = command.Title
            };

            await _unitOfWork.Repository<Post>().AddAsync(Post);
            Post.AddDomainEvent(new PostCreatedEvent(Post));

            await _unitOfWork.Save(cancellationToken);

            return await Result<Guid>.SuccessAsync(Post.Id, "Post Created.");
        }
    }
}