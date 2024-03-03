using AutoMapper;
using BlogWebApplication.Application.Common.Mappings;
using BlogWebApplication.Application.Interfaces.Repositories;
using BlogWebApplication.Domain.Entities;
using BlogWebApplication.Shared.Implements;
using MediatR;

namespace BlogWebApplication.Application.Features.Players.Commands.CreatePlayer
{
    public class CreatePlayerCommand: IRequest<Result<int>>, IMapFrom<Post>
    {
        public string Name { get; set; }
        public int ShirtNo { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime? BirthDate { get; set; }
    }

    internal class CreatePlayerCommandHandler : IRequestHandler<CreatePlayerCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreatePlayerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper; 
        }

        public async Task<Result<int>> Handle(CreatePlayerCommand command, CancellationToken cancellationToken)
        {
            var player = new Post()
            {
                Name = command.Name,
                ShirtNo = command.ShirtNo,
                PhotoUrl = command.PhotoUrl,
                BirthDate = command.BirthDate
            };

            await _unitOfWork.Repository<Post>().AddAsync(player);
            player.AddDomainEvent(new PlayerCreatedEvent(player));

            await _unitOfWork.Save(cancellationToken);

            return await Result<int>.SuccessAsync(player.Id, "Player Created.");
        }
    }
}