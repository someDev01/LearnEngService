namespace Api.Requests;

public record UpdateRepetitionScoreRequest(
    Guid NoteId,
    bool IsCorrect);
