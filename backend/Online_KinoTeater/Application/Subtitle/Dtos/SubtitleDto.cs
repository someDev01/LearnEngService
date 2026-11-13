namespace Application.Subtitle.Dtos;

public record SubtitleDto(
    Guid Id,
    string LanguageCode,
    string Value,
    string Format);
