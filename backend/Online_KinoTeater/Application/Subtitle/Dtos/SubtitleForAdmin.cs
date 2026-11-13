namespace Application.Subtitle.Dtos;

public record SubtitleForAdmin(
    Guid Id,
    Guid VideoId,
    string VideoTitle,
    string LanguageCode,
    string Value,
    string Format);