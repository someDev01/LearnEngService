namespace Infrastructure.Dtos;

public record YoutubeApiResponse(List<ItemDto> Items);

public record ItemDto(SnippetDto Snippet, ContentDetailsDto ContentDetails);
public record SnippetDto(string Title);
public record ContentDetailsDto(string Duration);
