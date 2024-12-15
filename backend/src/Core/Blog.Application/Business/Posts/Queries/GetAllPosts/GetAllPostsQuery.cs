using Blog.Shared.Pagination;
using MediatR;

namespace Blog.Application.Business.Posts.Queries.GetAllPosts
{
    public class GetAllPostsQuery : IRequest<PaginatedResult<GetAllPostsDto>>
    {
        public GetAllPostsQuery()
        {
        }

        public GetAllPostsQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}