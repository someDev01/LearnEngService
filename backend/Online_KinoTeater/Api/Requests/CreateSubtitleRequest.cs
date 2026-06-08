namespace Api.Requests;

public record CreateSubtitleRequest(
    Guid ContentId,
    string Language,
    string File,
    string Format);
