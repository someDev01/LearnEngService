using Application.SharedDtos;

namespace Application.Requests.Vocabulary;

public record VocabularyRequestDto(
    string Text,
    string? Context = null,
    List<string?> Translations = null,
    List<ExampleDto>? Example = null);
