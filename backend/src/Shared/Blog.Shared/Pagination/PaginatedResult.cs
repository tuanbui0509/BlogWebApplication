using Blog.Shared.Result;

namespace Blog.Shared.Pagination
{
    public class PaginatedResult<T> : TResult<IEnumerable<T>>
    {
        public PaginatedResult(IEnumerable<T> data, PaginationMetadata metadata, bool success, string message, int statusCode)
        {
            Data = data;
            Metadata = metadata;
            Success = success;
            Message = message;
            StatusCode = statusCode;
        }

        public PaginationMetadata Metadata { get; }
        public IEnumerable<T> Data { get; }
        public bool Success { get; }
        public string Message { get; }
        public int StatusCode { get; }

        // Factory Method: Success
        public static PaginatedResult<T> Successful(IEnumerable<T> data, int currentPage, int totalItems, int pageSize)
        {
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var metadata = new PaginationMetadata
            {
                CurrentPage = currentPage,
                TotalPages = totalPages,
                TotalItems = totalItems,
                PageSize = pageSize
            };

            return new PaginatedResult<T>(data, metadata, true, "Data retrieved successfully", 200);
        }

        // Factory Method: Failure
        public static PaginatedResult<T> Failure(string message, int statusCode = 400)
        {
            return new PaginatedResult<T>(null, null, false, message, statusCode);
        }
    }
}
