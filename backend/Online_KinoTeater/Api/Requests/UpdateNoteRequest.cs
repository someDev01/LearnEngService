using Application.SharedDtos;

namespace Api.Requests;

public record UpdateNoteRequest(
    Guid NoteId,
    string Word,
    List<string>? Translations,
    List<ExampleDto>? Examples);
