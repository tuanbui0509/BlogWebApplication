using Blog.Application.Dtos.Audit;
using Blog.Domain.Enums;

namespace Blog.Application.Dtos.Post
{
    public class PostDto: AuditDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string PostContents { get; set; }
        public string Slug { get; set; }
        public PublishState IsPublished { get; set; } = PublishState.Draft;
        public List<string> Tags { get; set; }
    }
}