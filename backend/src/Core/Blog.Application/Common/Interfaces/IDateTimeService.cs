namespace Blog.Application.Common.Interfaces
{
    public interface IDateTimeService
    {
        DateTime NowUtc { get; }
    }
}