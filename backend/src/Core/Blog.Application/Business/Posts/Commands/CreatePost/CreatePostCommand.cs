using System.Text.Json.Serialization;
using Blog.Application.Common.Mappings;
using Blog.Domain.Entities;
using Blog.Domain.Enums;
using MediatR;

namespace Blog.Application.Business.Posts.Commands.CreatePost
{
    public class CreatePostCommand : IRequest<Guid>, IMapFrom<Post>
    {
        public string Title { get; set; }
        public string PostContents { get; set; }
        public string Slug { get; set; }
        // public DateTime PublishDate { get; set; } after
        public PublishState IsPublished { get; set; }
        public List<string> Tags { get; set; }

        [JsonIgnore]
        public string UserId { get; set; }

        [JsonIgnore]
        public string UserName { get; set; }
    }
}