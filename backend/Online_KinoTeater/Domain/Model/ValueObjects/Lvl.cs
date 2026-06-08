using Domain.Model.Common;

namespace Domain.Model.ValueObjects;

public record Lvl(string Value): ValueObject
{
    private readonly static string[] _availableLvls = 
    [
        "A1",
        "A2",
        "B1",
        "B2",
        "C1",
        "C2"
    ];
    public static Result<Lvl> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result<Lvl>.Failure("Уровень слова не указан");

        if (!_availableLvls.Contains(value))
            return Result<Lvl>.Failure("Некорректное значение уровня слова");

        return Result<Lvl>.Success(new Lvl(value));
    }
}
