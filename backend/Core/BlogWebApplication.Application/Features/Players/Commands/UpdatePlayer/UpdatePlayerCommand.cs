using AutoMapper;
using BlogWebApplication.Application.Interfaces.Repositories;
using BlogWebApplication.Domain.Entities;
using BlogWebApplication.Shared.Implements;
using MediatR;

namespace BlogWebApplication.Application.Features.Players.Commands.UpdatePlayer
{
    public class UpdatePlayerCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ShirtNo { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime? BirthDate { get; set; }
    }

    internal class UpdatePlayerCommandHandler : IRequestHandler<UpdatePlayerCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdatePlayerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper; 
        }

        public async Task<Result<int>> Handle(UpdatePlayerCommand command, CancellationToken cancellationToken)
        {
            var player = await _unitOfWork.Repository<Post>().GetByIdAsync(command.Id);
            if (player != null)
            {
                player.Name = command.Name;
                player.ShirtNo = command.ShirtNo;
                player.PhotoUrl = command.PhotoUrl;
                player.BirthDate = command.BirthDate;

                await _unitOfWork.Repository<Post>().UpdateAsync(player);
                player.AddDomainEvent(new PlayerUpdatedEvent(player));

                await _unitOfWork.Save(cancellationToken);

                return await Result<int>.SuccessAsync(player.Id, "Player Updated.");
            }
            else
            {
                return await Result<int>.FailureAsync("Player Not Found.");
            }               
        }
    }
}