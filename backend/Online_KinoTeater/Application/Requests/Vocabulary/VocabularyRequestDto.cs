namespace Application.Requests.Vocabulary;

public record VocabularyRequestDto(
    string Text,
    string? Context = null,
    bool IsIncludedTranslations = true,
    bool IsIncludedExamples = true);
