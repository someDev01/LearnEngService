namespace Api.Requests;

public record CreateSubtitlesRequest(
    Guid VideoId,
    string LanguageCode,
    string Format);
