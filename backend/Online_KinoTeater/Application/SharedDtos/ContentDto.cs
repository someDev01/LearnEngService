namespace Application.SharedDtos;

public record ContentDto(
    Guid Id,
    string OriginalName,
    string TranslatedName);
