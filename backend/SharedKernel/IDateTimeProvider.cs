namespace SharedKernel;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
    public DateTime Now
    {
        get
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
        }
    }

}
