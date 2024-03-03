using BlogWebApplication.Domain.Common;
using BlogWebApplication.Domain.Entities;

namespace BlogWebApplication.Application.Features.Players.Commands.CreatePlayer
{
    public class PlayerCreatedEvent : BaseEvent
    {
        public Post Player { get; }

        public PlayerCreatedEvent(Post player)
        {
            Player = player;
        }
    }
}