namespace Blog.Shared.Result
{
    public interface IResult
    {
        bool Success { get; }
        string Message { get; }
        int StatusCode { get; }
    }

    // TResult - Base generic result interface
    public interface TResult<T> : IResult
    {
        T Data { get; }
    }
}