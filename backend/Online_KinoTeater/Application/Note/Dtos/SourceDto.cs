using Application.SharedDtos;

namespace Application.Note.Dtos;

public record SourceDto(
    Guid YoutubeVideoId,
    string YoutubeId,
    string YoutubeVideoTitle,
    string Context,
    DurationContextDto DurationContext);
