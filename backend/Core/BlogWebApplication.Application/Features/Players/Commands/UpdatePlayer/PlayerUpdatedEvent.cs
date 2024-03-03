using BlogWebApplication.Domain.Common;
using BlogWebApplication.Domain.Entities;

namespace BlogWebApplication.Application.Features.Players.Commands.UpdatePlayer
{
    public class PlayerUpdatedEvent: BaseEvent
    {
        public Post Player { get; }

        public PlayerUpdatedEvent(Post player)
        {
            Player = player;
        }
    }
}