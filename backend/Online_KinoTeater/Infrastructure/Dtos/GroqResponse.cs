namespace Infrastructure.Dtos;

public record GroqResponse(List<Choice> choices);

public record Choice(Message message);

public record Message(string content);
