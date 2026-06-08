namespace Application.Common.Ttl;

public static class TtlHelper
{
    public static TimeSpan UntilNextUtcTime()
    {
        var now = DateTime.UtcNow;

        var nextReset = new DateTime(
            now.Year,
            now.Month,
            now.Day,
            3,
            0,
            0,
            DateTimeKind.Utc);

        if(now >= nextReset) 
            nextReset = nextReset.AddDays(1);

        return nextReset - now;
    }
}
