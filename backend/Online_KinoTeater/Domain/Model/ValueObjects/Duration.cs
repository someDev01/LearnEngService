using Domain.Model.Common;

namespace Domain.Model.ValueObjects;

public record Duration(int Hour, int Minutes, int Seconds): ValueObject
{
    public static Result<Duration> Create(int hour, int minutes, int seconds)
    {
        if (hour < 0 || minutes < 0 || seconds < 0)
            return Result<Duration>.Failure("Длительность не может отрицательной");

        if (hour > 60)
            return Result<Duration>.Failure("Некорректное значение часа");

        if (minutes > 60)
            return Result<Duration>.Failure("Некорректное значение минут");

        if (seconds > 60)
            return Result<Duration>.Failure("Некорректное значение секунд");

        return Result<Duration>.Success(new Duration(hour, minutes, seconds));
    }

    public override string ToString() => $"{Hour:00}:{Minutes:00}:{Seconds:00}";
    
}
