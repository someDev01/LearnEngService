using Domain.Model.Common;

namespace Domain.Model.ValueObjects;

public record VideoUrl(string Url): ValueObject
{
    public static Result<VideoUrl> Create(string videoUrl)
    {
        if (string.IsNullOrWhiteSpace(videoUrl))
            return Result<VideoUrl>.Failure("Путь к видео контенту не указан");

        return Result<VideoUrl>.Success(new VideoUrl(videoUrl));
    }
}
