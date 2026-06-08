using Application.SharedDtos;

namespace Application.YoutubeVideo.Dtos;

public record YoutubeVideosPreviewDto(
    Guid Id,
    string YoutubeId,
    string TitleVideo,
    DurationContextDto Duration,
    string LexicalComplexity);
