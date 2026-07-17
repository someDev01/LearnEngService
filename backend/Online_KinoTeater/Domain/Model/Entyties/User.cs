using Domain.Model.Common;
using Domain.Model.ValueObjects;

namespace Domain.Model.Entyties;

public enum Role
{
    User,
    Admin
}

public class User : Entity
{
    public Email Email { get; private set; }
    
    public Name Name { get; private set; }

    public Role? Role { get; private set; }

    public DateTime CreatedAt { get; private set; }

    protected User() { }

    private User(Email email, Name name, Role? role)
    {
        Email = email;
        Name = name;
        Role = role;
        CreatedAt = DateTime.UtcNow;
    }

    public static Result<User> Create(Email email, Role? role)
    {
        if (email is null)
            return Result<User>.Failure("email для создания пользователя обязателена");

        if (role is null)
            return Result<User>.Failure("Роль для пользователя не указана");

        var nameResult = Name.Create(email.Value);
        if(!nameResult.IsSuccess)
            return Result<User>.Failure(nameResult.Error!);

        var name = nameResult.Value;
        return Result<User>.Success(new User(email, name, role));
    }
}
