namespace Blog.Shared.Result
{
    public class FailureResult : IResult
    {
        public FailureResult(string message, int statusCode)
        {
            Message = message;
            StatusCode = statusCode;
            Success = false;
        }

        public FailureResult(string message)
        {
            Message = message;
            StatusCode = 400;
            Success = false;
        }

        public bool Success { get; }
        public string Message { get; }
        public int StatusCode { get; }
    }
}