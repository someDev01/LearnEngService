using Domain.Model.Common;

namespace Domain.Model.ValueObjects;

public record Context(string Sentence): ValueObject
{
    public static Result<Context> Create(string sentence)
    {
        if (string.IsNullOrWhiteSpace(sentence))
            return Result<Context>.Failure("Контекст не указан");

        return Result<Context>.Success(new Context(sentence));
    }
}
