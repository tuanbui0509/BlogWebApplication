namespace Blog.Application.Common.Exceptions
{
    // Custom exceptions
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }

    public class UnauthorizedAccessException : Exception
    {
        public UnauthorizedAccessException(string message) : base(message) { }
    }

    public class ServiceException : Exception
    {
        public ServiceException(string message) : base(message) { }
    }

}