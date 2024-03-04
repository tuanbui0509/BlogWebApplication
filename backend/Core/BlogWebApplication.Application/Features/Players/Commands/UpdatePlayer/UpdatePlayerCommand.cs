using AutoMapper;
using BlogWebApplication.Application.Interfaces.Repositories;
using BlogWebApplication.Domain.Entities;
using BlogWebApplication.Shared.Implements;
using MediatR;

namespace BlogWebApplication.Application.Features.Posts.Commands.UpdatePost
{
    public class UpdatePostCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
    }

    internal class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdatePostCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper; 
        }

        public async Task<Result<Guid>> Handle(UpdatePostCommand command, CancellationToken cancellationToken)
        {
            var Post = await _unitOfWork.Repository<Post>().GetByIdAsync(command.Id);
            if (Post != null)
            {
                Post.Title = command.Title;

                await _unitOfWork.Repository<Post>().UpdateAsync(Post);
                Post.AddDomainEvent(new PostUpdatedEvent(Post));

                await _unitOfWork.Save(cancellationToken);

                return await Result<Guid>.SuccessAsync(Post.Id, "Post Updated.");
            }
            else
            {
                return await Result<Guid>.FailureAsync("Post Not Found.");
            }               
        }
    }
}