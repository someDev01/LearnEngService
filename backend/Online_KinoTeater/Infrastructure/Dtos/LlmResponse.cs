namespace Infrastructure.Dtos;

public record LlmResponse(List<Choice> choices);

public record Choice(Message message);

public record Message(string content);
