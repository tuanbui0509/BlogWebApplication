using BlogWebApplication.Application.Interfaces;

namespace BlogWebApplication.Infrastructure.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}