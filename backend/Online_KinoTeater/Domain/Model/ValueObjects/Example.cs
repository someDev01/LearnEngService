using Domain.Model.Common;

namespace Domain.Model.ValueObjects;

public record Example(
    string? Text,
    string? Translate)
{
    public static Result<Example> Create(string? text, string? translateText)
    {
        return Result<Example>.Success(new Example(text, translateText));
    }
}
