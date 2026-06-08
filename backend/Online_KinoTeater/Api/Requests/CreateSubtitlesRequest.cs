namespace Api.Requests;

public record CreateSubtitlesRequest(
    Guid VideoId,
    string Language,
    string Format);
