using Domain.Model.Common;

namespace Domain.Model.ValueObjects;

public record LexicalComplexity(string Value): ValueObject
{
    private static readonly string _lvlA = "A1-A2";
    private static readonly string _lvlAb1 = "A2-B1";
    private static readonly string _lvlB = "B1-B2";
    private static readonly string _lvlBc1 = "B2-C1";
    private static readonly string _lvlC = "C1-C2";
    private static readonly string[] _allowedValues = { _lvlA, _lvlAb1, _lvlB, _lvlBc1, _lvlC };

    public static Result<LexicalComplexity> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result<LexicalComplexity>.Failure("Сложность лексики не указана!");

        if (!_allowedValues.Contains(value, StringComparer.OrdinalIgnoreCase))
            return Result<LexicalComplexity>.Failure($"Недопустимые значения лексики!");

        return Result<LexicalComplexity>.Success(new LexicalComplexity(value));
    }
}
