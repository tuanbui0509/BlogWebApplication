using AutoMapper;
using Blog.Application.Common.Interfaces.Repositories;
using Blog.Application.Business.Posts.Commands.CreatePost;
using Blog.Domain.Entities;
using MediatR;

namespace Blog.Application.Business.Posts.Handlers
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePostCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreatePostCommand command, CancellationToken cancellationToken)
        {

            var entity = new Post
            {
                Title = command.Title,
                Slug = command.Slug,
                PostContents = command.PostContents,
                CreatedBy = command.UserName,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = command.UserName,
                UpdatedDate = DateTime.UtcNow,
                AuthorId = command.UserId,
            };

            await _unitOfWork.Repository<Post>().AddAsync(entity);
            entity.AddDomainEvent(new PostCreatedEvent(entity));
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}