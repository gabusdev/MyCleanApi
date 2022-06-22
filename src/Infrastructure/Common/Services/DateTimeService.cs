using Application.Common.Interfaces;

namespace Infrastructure.Common.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.Now;
}
