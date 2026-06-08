namespace Application.Subtitle.Dtos;

public record SubtitleDto(
    Guid Id,
    string Language,
    string Value,
    string Format);
