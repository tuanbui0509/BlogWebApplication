using Blog.Shared.Result;

namespace Blog.Shared
{
    public class PaginatedResult<T> : IResult
    {
        public PaginatedResult(IEnumerable<T> data, int currentPage, int totalPages, int totalItems, int pageSize)
        {
            Data = data;
            CurrentPage = currentPage;
            TotalPages = totalPages;
            TotalItems = totalItems;
            PageSize = pageSize;
            Success = true;
            Message = "Data retrieved successfully";
            StatusCode = 200;
        }

        public IEnumerable<T> Data { get; }
        public int CurrentPage { get; }
        public int TotalPages { get; }
        public int TotalItems { get; }
        public int PageSize { get; }
        public bool Success { get; }
        public string Message { get; }
        public int StatusCode { get; }
    }
}