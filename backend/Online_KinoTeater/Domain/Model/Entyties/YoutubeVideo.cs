using Domain.Model.Common;
using Domain.Model.ValueObjects;

namespace Domain.Model.Entyties;

public class YoutubeVideo : Entity
{
    public string YoutubeId { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;
    public Duration Duration { get; private set; }
    public LexicalComplexity LexicalComplexity { get; private set; }
    public bool IsBlocked { get; private set; } = false;

    private YoutubeVideo() { }

    private YoutubeVideo(
        string youtubeId,
        string title,
        Duration duration,
        LexicalComplexity lexicalComplexity)
    {
        YoutubeId = youtubeId;
        Title = title;
        Duration = duration;
        LexicalComplexity = lexicalComplexity;
    }

    public static Result<YoutubeVideo> Create(
        string youtubeId,
        string title,
        Duration duration,
        LexicalComplexity lexicalComplexity)
    {
        if (string.IsNullOrWhiteSpace(youtubeId))
            return Result<YoutubeVideo>.Failure("Id youtube видео не указано");

        if (string.IsNullOrWhiteSpace(title))
            return Result<YoutubeVideo>.Failure("Название youtube видео не указано");

        if (duration is null)
            return Result<YoutubeVideo>.Failure("Длительность для youtube видео не указана");

        return Result<YoutubeVideo>.Success(new YoutubeVideo(youtubeId, title, duration, lexicalComplexity));
    }

    public Result UpdateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result.Failure("Название youtube видео не указано!");

        Title = title;
        return Result.Success();
    }
    public Result UpdateLexicalComplexity(string complexity)
    {
        if (complexity is null)
            return Result.Failure("Сложность лексики не задана");

        var lexicalComplexity = LexicalComplexity.Create(complexity);
        if (!lexicalComplexity.IsSuccess)
            return Result.Failure(lexicalComplexity.Error!);

        LexicalComplexity = lexicalComplexity.Value!;
        return Result.Success();
    }
    public Result UpdateDuration(int hour, int minutes, int seconds)
    {
        var duration = Duration.Create(hour, minutes, seconds);
        if (!duration.IsSuccess)
            return Result.Failure(duration.Error!);

        Duration = duration.Value!;
        return Result.Success();
    }
    public Result SetBlocked(bool isBlocked)
    {
        IsBlocked = isBlocked;
        return Result.Success();
    }

}
