namespace Blog.Shared.Result
{
    public class DataResult<T> : TResult<T>
    {
        public DataResult(T data, string message, int statusCode = 200, bool isSuccess = true)
        {
            Data = data;
            Message = message;
            StatusCode = statusCode;
            Success = isSuccess;
        }

        public bool Success { get; }
        public string Message { get; }
        public int StatusCode { get; }
        public T Data { get; }
    }
}