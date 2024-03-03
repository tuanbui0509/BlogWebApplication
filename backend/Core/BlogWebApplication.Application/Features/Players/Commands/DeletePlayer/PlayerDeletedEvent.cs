using BlogWebApplication.Domain.Common;
using BlogWebApplication.Domain.Entities;

namespace BlogWebApplication.Application.Features.Players.Commands.DeletePlayer
{
    public class PlayerDeletedEvent : BaseEvent
    {
        public Post Player { get; }

        public PlayerDeletedEvent(Post player)
        {
            Player = player;
        }
    }
}