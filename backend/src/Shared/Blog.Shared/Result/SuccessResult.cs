namespace Blog.Shared.Result
{
    public class SuccessResult : IResult
    {
        public SuccessResult(string message, int statusCode)
        {
            Message = message;
            StatusCode = statusCode;
            Success = true;
        }

        public bool Success { get; }
        public string Message { get; }
        public int StatusCode { get; }
    }
}