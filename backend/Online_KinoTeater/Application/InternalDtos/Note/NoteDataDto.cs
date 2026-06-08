using Application.SharedDtos;

namespace Application.InternalDtos.Translated;

public record NoteDataDto(
    string Word,
    List<string> Translations,
    string Transcription,
    List<ExampleDto> Examples,
    string Level);
