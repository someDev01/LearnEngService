using Application.SharedDtos;

namespace Api.Requests;

public record CreateNoteRequest(
    string Word,
    List<string> Translations,
    List<ExampleDto> Examples);
