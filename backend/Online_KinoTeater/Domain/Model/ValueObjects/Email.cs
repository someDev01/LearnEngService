using Domain.Model.Common;
using System.Text.RegularExpressions;

namespace Domain.Model.ValueObjects;

public record Email(string Value): ValueObject
{
    public static Result<Email> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result<Email>.Failure("Email не указан");

        if (!Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            return Result<Email>.Failure("Некорректный формат email");

        return Result<Email>.Success(new Email(value.Trim().ToLower()));
    }
}
