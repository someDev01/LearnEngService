using Domain.Model.Common;

namespace Domain.Model.ValueObjects;

public record Name(string Value): ValueObject
{
    public static Result<Name> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result<Name>.Failure("Имя не указано");

        string name = email.Split('@')[0].Trim();
        
        return Result<Name>.Success(new Name(name));
    }
}