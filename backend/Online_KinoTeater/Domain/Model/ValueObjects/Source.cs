using Domain.Model.Common;

namespace Domain.Model.ValueObjects;

public record Source(
    Guid YoutubeVideoId,
    string YoutubeId,
    string YoutubeVideoTitle,
    string Context,
    int Hours,
    int Minutes,
    int Seconds): ValueObject
{
    public static Result<Source> Create(
        Guid youtubeVideoId,
        string youtubeId,
        string youtubeVideoTitle,
        string context,
        int hours = 0,
        int minutes = 0,
        int seconds = 0)
    {
        if (string.IsNullOrWhiteSpace(context))
            return Result<Source>.Failure("Контекст не указан");

        return Result<Source>.Success(new Source(
            youtubeVideoId,
            youtubeId,
            youtubeVideoTitle, 
            context, 
            hours, 
            minutes, 
            seconds));
    }
}
