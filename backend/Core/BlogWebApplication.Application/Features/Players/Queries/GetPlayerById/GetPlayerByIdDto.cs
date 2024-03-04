using BlogWebApplication.Application.Common.Mappings;
using BlogWebApplication.Domain.Entities;

namespace BlogWebApplication.Application.Features.Posts.Queries.GetPostById
{
    public class GetPostByIdDto: IMapFrom<Post>
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public int ShirtNo { get; init; }
        public int HeightInCm { get; init; }
        public string FacebookUrl { get; init; }
        public string TwitterUrl { get; init; }
        public string InstagramUrl { get; init; }
        public int DisplayOrder { get; init; }
    }
}