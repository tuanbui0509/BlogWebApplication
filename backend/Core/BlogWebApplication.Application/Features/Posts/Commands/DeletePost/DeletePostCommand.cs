using AutoMapper;
using BlogWebApplication.Application.Common.Mappings;
using BlogWebApplication.Application.Interfaces.Repositories;
using BlogWebApplication.Domain.Entities;
using BlogWebApplication.Shared.Implements;
using MediatR;

namespace BlogWebApplication.Application.Features.Posts.Commands.DeletePost
{
    public class DeletePostCommand : IRequest<Result<Guid>>, IMapFrom<Post>
    {
        public Guid Id { get; set; }

        public DeletePostCommand()
        {

        }

        public DeletePostCommand(Guid id)
        {
            Id = id;
        }
    }
    internal class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeletePostCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<Guid>> Handle(DeletePostCommand command, CancellationToken cancellationToken)
        {
            var Post = await _unitOfWork.Repository<Post>().GetByIdAsync(command.Id);
            if (Post != null)
            {
                await _unitOfWork.Repository<Post>().DeleteAsync(Post);
                Post.AddDomainEvent(new PostDeletedEvent(Post));

                await _unitOfWork.Save(cancellationToken);

                return await Result<Guid>.SuccessAsync(Post.Id, "Product Deleted");
            }
            else
            {
                return await Result<Guid>.FailureAsync("Post Not Found.");
            }
        }
    }
}