namespace Application.Common.Time;

public static class Time
{
    public static TimeSpan ConvertToTimeSeconds(int time) =>
        TimeSpan.FromSeconds(time);
}
