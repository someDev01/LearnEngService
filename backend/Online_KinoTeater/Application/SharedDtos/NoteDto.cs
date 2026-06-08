using Application.Note.Dtos;

namespace Application.SharedDtos;

public record NoteDto(
    Guid Id,
    string Word,
    List<string> Translations,
    string Transcription,
    string? Lvl,
    List<ExampleDto> Examples,
    int RepetitionScore,
    SourceDto? Source,
    DateTime? LastTrainedAt,
    DateTime CreatedAt);
