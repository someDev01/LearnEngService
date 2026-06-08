namespace Application.Subtitle.Dtos;

public record VideoPlayerResponseDto(
    string YoutubeVideoTitle,
    string YoutubeId,
    List<SubtitleDto> Subtitles);
